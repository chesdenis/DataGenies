using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DataGenies.Core.Behaviours;
using DataGenies.Core.Configurators;
using DataGenies.Core.Containers;
using DataGenies.Core.Models;
using DataGenies.Core.Publishers;
using DataGenies.Core.Receivers;
using DataGenies.Core.Tests.Integration.Extensions;
using DataGenies.Core.Tests.Integration.Mocks.ApplicationTemplates;
using DataGenies.Core.Tests.Integration.Mocks.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataGenies.Core.Tests.Integration.Services
{
    [TestClass]
    public class ApplicationTemplatesTests : BaseIntegrationTest
    {
        [TestInitialize]
        public override void Initialize()
        {
            base.Initialize();
            
            ApplicationTemplatesScanner.RegisterMockApplicationTemplate(typeof(DynamicMessagesPublisher),
                "DynamicMessagesPublisherTemplate");
            
            ApplicationTemplatesScanner.RegisterMockApplicationTemplate(typeof(MockSimpleReceiver),
                "SimpleReceiverTemplate");
        }

        [TestMethod]
        public void ShouldReadSettingsAndUseThem()
        {
            // Arrange
            var publisherId = 1;
            InMemorySchemaDataBuilder
                .UsingConfigTemplate(new
                {
                    PropertyA = "PhraseTemplate",
                    PropertyB = "",
                    SectionA = new
                    {
                        InnerProperty1 = "[SectionA_InnerProperty1]",
                        InnerProperty2 = "[SectionA_InnerProperty2]"
                    },
                    SectionB = new
                    {
                        InnerProperty1 = "fixed setting",
                        InnerProperty2 = "fixed setting"
                    }
                })
                .UsingParametersDict(new Dictionary<string, string>()
                {
                    {"SectionA_InnerProperty1", "10"},
                    {"SectionA_InnerProperty2", "20"}
                })
                .CreateApplicationTemplate(
                    "DynamicMessagesPublisherTemplate",
                    "2019.1.1")
                .CreateApplicationInstance("SampleAppPublisher", publisherId)
                .ResetScopedConfigAndParameters();
            
            var receiverId = 2;
            InMemorySchemaDataBuilder.CreateApplicationTemplate(
                    "SimpleReceiverTemplate",
                    "2018.1.1")
                .CreateApplicationInstance("SampleAppReceiver", receiverId);
            
            InMemorySchemaDataBuilder.ConfigureBinding(
                "SampleAppPublisher",
                "SampleAppReceiver", "#");
            
            Orchestrator.Deploy(publisherId);
            Orchestrator.Deploy(receiverId);
            
            // Act
            Orchestrator.Start(publisherId);
            Orchestrator.Start(receiverId);
            
            Task.Run(async () =>
            {
                await Task.Delay(1000);
                await Orchestrator.Stop(receiverId);
            }).Wait();
            
            // Assert
            var publisherProperties = Orchestrator.GetApplicationInstanceContainer(publisherId).Resolve<MockPublisherProperties>();
            var receiverProperties = Orchestrator.GetApplicationInstanceContainer(receiverId).Resolve<MockReceiverProperties>();
    
            Assert.AreEqual(10, publisherProperties.PublishedMessages.Count);
            Assert.AreEqual(10, receiverProperties.ReceivedMessages.Count);
            
            Assert.AreEqual("TestString4 - InnerProperty2: 20", publisherProperties.PublishedMessages[4]);
            Assert.AreEqual("TestString4 - InnerProperty2: 20", receiverProperties.ReceivedMessages[4]); 
        }

        private class DynamicMessagesPublisher : MockSimplePublisher
        {
            public DynamicMessagesPublisher(IContainer container, IPublisher publisher, IReceiver receiver, IEnumerable<BehaviourTemplate> behaviourTemplates, IEnumerable<WrapperBehaviourTemplate> wrapperBehaviours, ISchemaDataContext schemaDataContext, IBindingConfigurator bindingConfigurator, MockSettings settings) : base(container, publisher, receiver, behaviourTemplates, wrapperBehaviours, schemaDataContext, bindingConfigurator)
            {
                Settings = settings;
            }

            private MockSettings Settings { get; set; }

         
            protected override void OnStart()
            {
                Settings = ReadSettings<MockSettings>();
                
                for (int i = 0; i < int.Parse(Settings.SectionA.InnerProperty1); i++)
                {
                    var testString = $"TestString{i} - InnerProperty2: {Settings.SectionA.InnerProperty2}";
            
                    var testData = Encoding.UTF8.GetBytes(testString);
                    var mqMessage = new MqMessage()
                    {
                        Body = testData
                    };
                    this.Publish(mqMessage);
    
                    Properties.PublishedMessages.Add(Encoding.UTF8.GetString(mqMessage.Body));
                }
               
            }
        }
    }
}