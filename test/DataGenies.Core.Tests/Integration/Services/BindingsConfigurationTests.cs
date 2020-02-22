using System.Collections.Generic;
using System.Threading.Tasks;
using DataGenies.Core.Behaviours;
using DataGenies.Core.Containers;
using DataGenies.Core.Publishers;
using DataGenies.Core.Receivers;
using DataGenies.Core.Tests.Integration.Extensions;
using DataGenies.Core.Tests.Integration.Mocks.ApplicationTemplates;
using DataGenies.Core.Tests.Integration.Mocks.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataGenies.Core.Tests.Integration.Services
{
    [TestClass]
    public class BindingsConfigurationTests : BaseIntegrationTest
    {
         [TestInitialize]
        public override void Initialize()
        {
            base.Initialize();
            
            ApplicationTemplatesScanner.RegisterMockApplicationTemplate(typeof(MockSimplePublisher),
                "MockSimplePublisher");
            
            ApplicationTemplatesScanner.RegisterMockApplicationTemplate(typeof(MockSimpleReceiver),
                "SimpleReceiverTemplate");
            
            ApplicationTemplatesScanner.RegisterMockApplicationTemplate(typeof(MockSimpleReceiver),
                "NotifierReceiverTemplate");
        }

        [TestMethod]
        public void ShouldUseCustomExchangeIfDesigned()
        {
            // Arrange
            var publisherId = 1;
            InMemorySchemaDataBuilder
                .CreateApplicationTemplate(
                    "DynamicMessagesPublisherTemplate",
                    "2019.1.1")
                .CreateApplicationInstance("SampleAppPublisher", publisherId)
                .ResetScopedConfigAndParameters();
            
            var receiverAId = 2;
            InMemorySchemaDataBuilder.CreateApplicationTemplate(
                    "SimpleReceiverTemplate",
                    "2018.1.1")
                .CreateApplicationInstance("SampleAppReceiver", receiverId);
            
            var receiverBId = 2;
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
        
        private class CustomMessagesPublisher : MockSimplePublisher
        {
            public CustomMessagesPublisher(IContainer container, IPublisher publisher, IReceiver receiver,
                IEnumerable<BehaviourTemplate> behaviourTemplates,
                IEnumerable<WrapperBehaviourTemplate> wrapperBehaviours) : base(container, publisher, receiver,
                behaviourTemplates, wrapperBehaviours)
            {
            }

            protected override void OnStart()
            {
                // Settings = ReadSettings<MockSettings>();
                //
                // for (int i = 0; i < int.Parse(Settings.SectionA.InnerProperty1); i++)
                // {
                //     var testString = $"TestString{i} - InnerProperty2: {Settings.SectionA.InnerProperty2}";
                //
                //     var testData = Encoding.UTF8.GetBytes(testString);
                //     var mqMessage = new MqMessage()
                //     {
                //         Body = testData
                //     };
                //     this.Publish(mqMessage);
                //
                //     Properties.PublishedMessages.Add(Encoding.UTF8.GetString(mqMessage.Body));
                // }
               
            }
        }
    }
}