using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DataGenies.Core.Behaviours;
using DataGenies.Core.Containers;
using DataGenies.Core.Publishers;
using DataGenies.Core.Tests.Integration.Extensions;
using DataGenies.Core.Tests.Integration.Mocks.ApplicationTemplates;
using DataGenies.Core.Tests.Integration.Mocks.Properties;
using DataGenies.Core.Wrappers;
using DataGenies.InMemory;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataGenies.Core.Tests.Integration.Behaviours
{
    [TestClass]
    public class BeforeStartBehaviourTests : BaseIntegrationTest
    {
        [TestInitialize]
        public override void Initialize()
        {
            base.Initialize();
            
            ApplicationTemplatesScanner.RegisterMockApplicationTemplate(typeof(MessageWithPrefixPublisher),"SampleAppPublisherTemplate");
            ApplicationTemplatesScanner.RegisterMockApplicationTemplate(typeof(MockSimpleReceiver), "SampleAppReceiverTemplate");
            
            Behaviours.Add("SampleBehaviour", new MarkMessagesWithPrefixBehaviour());
        }
        
        [TestMethod]
        public void BeforeStartBehaviourShouldAffectApplication()
        {
            // Arrange
            var publisherId = 1;
            InMemorySchemaDataBuilder.CreateApplicationTemplate(
                    "SampleAppPublisherTemplate", 
                    "2019.1.1")
                .CreateApplicationInstance("SampleAppPublisher", publisherId)
                .RegisterBehaviour("SampleBehaviour", "2019.1.1")
                .ApplyBehaviour();
        
            var receiverId = 2;
            InMemorySchemaDataBuilder.CreateApplicationTemplate(
                    "SampleAppReceiverTemplate",
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
            
            Assert.AreEqual("PrefixTestString", publisherProperties.PublishedMessages[0]);
            Assert.AreEqual("PrefixTestString", receiverProperties.ReceivedMessages[0]);
        }
        
        private class MessageWithPrefixPublisher : MockSimplePublisher
        {
            public MessageWithPrefixPublisher(IContainer container, IPublisher publisher,
                IEnumerable<IBasicBehaviour> basicBehaviours, IEnumerable<IBehaviourOnException> behaviourOnExceptions,
                IEnumerable<IWrapperBehaviour> wrapperBehaviours) : base(container, publisher, basicBehaviours,
                behaviourOnExceptions, wrapperBehaviours)
            {
            }

            protected override void OnStart()
            {
                var testString = $"{this.Properties.ManagedParameter}TestString";
            
                var testData = Encoding.UTF8.GetBytes(testString);
                this.Publish(new MqMessage()
                {
                    Body = testData
                });
            
                Properties.PublishedMessages.Add(testString);
            }
        }
        
        private class MarkMessagesWithPrefixBehaviour : BasicBehaviour
        {
            public override void Execute(IContainer arg)
            {
                arg.Resolve<MockPublisherProperties>().ManagedParameter = "Prefix";
            }

            public override BehaviourScope BehaviourScope { get; set; } = BehaviourScope.Service;
            public override BehaviourType BehaviourType { get; set; } = BehaviourType.BeforeStart;
        }
    }
}