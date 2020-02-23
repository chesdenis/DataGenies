using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataGenies.Core.Behaviours;
using DataGenies.Core.Configurators;
using DataGenies.Core.Containers;
using DataGenies.Core.Extensions;
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
    public class BindingsConfigurationTests : BaseIntegrationTest
    {
         [TestInitialize]
        public override void Initialize()
        {
            base.Initialize();
            
            ApplicationTemplatesScanner.RegisterMockApplicationTemplate(typeof(CustomMessagesPublisher),
                "CustomMessagesPublisherTemplate");
            
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
                    "CustomMessagesPublisherTemplate",
                    "2019.1.1")
                .CreateApplicationInstance("Publisher", publisherId)
                .ResetScopedConfigAndParameters();
            
            var receiverId = 2;
            InMemorySchemaDataBuilder.CreateApplicationTemplate(
                    "SimpleReceiverTemplate",
                    "2018.1.1")
                .CreateApplicationInstance("Receiver", receiverId);
            
            var notifierId = 3;
            InMemorySchemaDataBuilder.CreateApplicationTemplate(
                    "SimpleReceiverTemplate",
                    "2018.1.1")
                .CreateApplicationInstance("Notifier", notifierId);
            
            InMemorySchemaDataBuilder.ConfigureBinding(
                "Publisher",
                "Receiver", "#");    
            
            InMemorySchemaDataBuilder.ConfigureBinding(
                "Publisher",
                "Notifier", "#");
            
            Orchestrator.Deploy(publisherId);
            Orchestrator.Deploy(receiverId);
            Orchestrator.Deploy(notifierId);
            
            // Act
            Orchestrator.Start(publisherId);
            Orchestrator.Start(receiverId);
            Orchestrator.Start(notifierId);
            
            Task.Run(async () =>
            {
                await Task.Delay(1000);
                await Orchestrator.Stop(receiverId);
                await Orchestrator.Stop(notifierId);
            }).Wait();
            
            // Assert
            var publisherProperties = Orchestrator.GetApplicationInstanceContainer(publisherId).Resolve<MockPublisherProperties>();
            var receiverProperties = Orchestrator.GetApplicationInstanceContainer(receiverId).Resolve<MockReceiverProperties>();
            var notifierProperties = Orchestrator.GetApplicationInstanceContainer(notifierId).Resolve<MockReceiverProperties>();
            
            Assert.AreEqual(2, publisherProperties.PublishedMessages.Count);
            Assert.AreEqual(1, receiverProperties.ReceivedMessages.Count);
            Assert.AreEqual(1, notifierProperties.ReceivedMessages.Count);
            
            Assert.AreEqual("\"TestString\"", publisherProperties.PublishedMessages[0]);
            Assert.AreEqual("\"TestString\"", receiverProperties.ReceivedMessages[0]); 
            Assert.AreEqual("\"NotifierText\"", notifierProperties.ReceivedMessages[0]); 
        }
        
        private class CustomMessagesPublisher : MockSimplePublisher
        {
            public CustomMessagesPublisher(IContainer container, IPublisher publisher, IReceiver receiver,
                IEnumerable<BehaviourTemplate> behaviourTemplates,
                IEnumerable<WrapperBehaviourTemplate> wrapperBehaviours, BindingNetwork bindingNetwork) : base(
                container, publisher, receiver, behaviourTemplates, wrapperBehaviours, bindingNetwork)
            {
            }

            protected override void OnStart()
            {
                var mqMessage = new MqMessage
                {
                    Body = "TestString".ToBytes(),
                    RoutingKey = "#"
                };

               this.ConnectedReceivers()
                    .Except(
                    this.ConnectedReceivers(w => w.ReceiverInstanceName == "Notifier"))
                    .ManagedPublishUsing(this, mqMessage);
                 
                var mqMessageToNotifier = new MqMessage
                {
                    Body = "NotifierText".ToBytes(),
                    RoutingKey = "#"
                };
                
                this.ConnectedReceivers("Notifier")
                    .ManagedPublishUsing(this, mqMessageToNotifier);
                 
                Properties.PublishedMessages.Add(Encoding.UTF8.GetString(mqMessage.Body));
                Properties.PublishedMessages.Add(Encoding.UTF8.GetString(mqMessageToNotifier.Body));
            }
        }
    }
}