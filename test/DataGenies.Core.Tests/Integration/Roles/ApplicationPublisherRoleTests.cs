using System.Threading.Tasks;
using DataGenies.Core.Models;
using DataGenies.Core.Tests.Integration.Mocks.ApplicationTemplates;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace DataGenies.Core.Tests.Integration.Roles
{
    [TestClass]
    public class ApplicationPublisherRoleTests : BaseIntegrationTest
    {
        [TestInitialize]
        public override void Initialize()
        {
            base.Initialize();

            ApplicationTemplatesScanner.FindType(
                    Arg.Is<ApplicationTemplateEntity>(w => w.Name == "SampleAppPublisherTemplate"))
                .Returns(typeof(MockSimplePublisher));
            ApplicationTemplatesScanner.FindType(
                    Arg.Is<ApplicationTemplateEntity>(w => w.Name == "SampleAppReceiverTemplate"))
                .Returns(typeof(MockSimpleReceiver));
            
            ApplicationTemplatesScanner.FindType(
                    Arg.Is<ApplicationTemplateEntity>(w => w.Name == "SampleAppPublisherMultipleMessagesDifferentRoutingKeysTemplate"))
                .Returns(typeof(MockPublisherMultipleMessagesDifferentRoutingKeys));
        }

        [TestMethod]
        public void PublisherRoleShouldDeliverMessagesToReceiver()
        {
            // Arrange
            var publisherId = 1;
            InMemorySchemaDataBuilder.CreateApplicationTemplate(
                    "SampleAppPublisherTemplate", 
                    "2019.1.1")
                .CreateApplicationInstance("SampleAppPublisher", publisherId);
            
            var receiverId = 2;
            InMemorySchemaDataBuilder.CreateApplicationTemplate(
                    "SampleAppReceiverTemplate", 
                    "2018.1.1", receiverId)
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
            // var publisherComponent = (IApplicationWithContext) Orchestrator.GetManagedApplicationInstance(publisherId).GetRootComponent();
            // var publisherProperties = publisherComponent.ContextContainer.Resolve<MockPublisherProperties>();
            //
            // var receiverComponent = (IApplicationWithContext) Orchestrator.GetManagedApplicationInstance(receiverId).GetRootComponent();;
            // var receiverProperties = receiverComponent.ContextContainer.Resolve<MockReceiverProperties>();
            //
            // Assert.AreEqual("TestString", publisherProperties.PublishedMessages[0]);
            // Assert.AreEqual("TestString", receiverProperties.ReceivedMessages[0]);
        }
        
        [TestMethod]
        public void PublisherRoleShouldDeliverOneMessageToMultipleReceivers()
        {
            // Arrange
            var publisherId = 1;
            InMemorySchemaDataBuilder.CreateApplicationTemplate(
                    "SampleAppPublisherTemplate", 
                    "2019.1.1", publisherId)
                .CreateApplicationInstance("SampleAppPublisher");
            
            var receiverAId = 2;
            InMemorySchemaDataBuilder.CreateApplicationTemplate(
                    "SampleAppReceiverTemplate", 
                    "2018.1.1", receiverAId)
                .CreateApplicationInstance("SampleAppReceiverA");
            
            var receiverBId = 2;
            InMemorySchemaDataBuilder.CreateApplicationTemplate(
                    "SampleAppReceiverTemplate", 
                    "2018.1.1", receiverBId)
                .CreateApplicationInstance("SampleAppReceiverB");
        
            InMemorySchemaDataBuilder.ConfigureBinding(
                "SampleAppPublisher", 
                "SampleAppReceiverA", "#");
            
            InMemorySchemaDataBuilder.ConfigureBinding(
                "SampleAppPublisher", 
                "SampleAppReceiverB", "#");
             
            Orchestrator.Deploy(publisherId);
            Orchestrator.Deploy(receiverAId);
            Orchestrator.Deploy(receiverBId);

            // Act
            Orchestrator.Start(publisherId);
            Orchestrator.Start(receiverAId);
            Orchestrator.Start(receiverBId);

            Task.Run(async () =>
            {
                await Task.Delay(1000);
                await Orchestrator.Stop(receiverAId);
                await Orchestrator.Stop(receiverBId);
            }).Wait();
                
            // Assert
            // var publisherComponent = (IApplicationWithContext) Orchestrator.GetManagedApplicationInstance(publisherId).GetRootComponent();
            // var publisherProperties = publisherComponent.ContextContainer.Resolve<MockPublisherProperties>();
            //
            // var receiverAComponent = (IApplicationWithContext) Orchestrator.GetManagedApplicationInstance(receiverAId).GetRootComponent();;
            // var receiverAProperties = receiverAComponent.ContextContainer.Resolve<MockReceiverProperties>();
            //
            // var receiverBComponent = (IApplicationWithContext) Orchestrator.GetManagedApplicationInstance(receiverBId).GetRootComponent();;
            // var receiverBProperties = receiverBComponent.ContextContainer.Resolve<MockReceiverProperties>();
            //
            // Assert.AreEqual(1, receiverAProperties.ReceivedMessages.Count);
            // Assert.AreEqual(1,receiverBProperties.ReceivedMessages.Count);
            //
            // Assert.AreEqual("TestString", publisherProperties.PublishedMessages[0]);
            // Assert.AreEqual("TestString",receiverAProperties.ReceivedMessages[0]);
            // Assert.AreEqual("TestString", receiverBProperties.ReceivedMessages[0]);
        }
        
        [TestMethod]
        public void PublisherRoleShouldDeliverMultipleMessagesToMultipleReceivers()
        {
            // Arrange
            var publisherId = 1;
            InMemorySchemaDataBuilder.CreateApplicationTemplate(
                    "SampleAppPublisherMultipleMessagesDifferentRoutingKeysTemplate", 
                    "2019.1.1")
                .CreateApplicationInstance("SampleAppPublisher", publisherId);
            
            var receiverAId = 2;
            InMemorySchemaDataBuilder.CreateApplicationTemplate(
                    "SampleAppReceiverTemplate", 
                    "2018.1.1")
                .CreateApplicationInstance("SampleAppReceiverA", receiverAId);
            
            var receiverBId = 3;
            InMemorySchemaDataBuilder.CreateApplicationTemplate(
                    "SampleAppReceiverTemplate", 
                    "2018.1.1")
                .CreateApplicationInstance("SampleAppReceiverB", receiverBId);
        
            InMemorySchemaDataBuilder.ConfigureBinding(
                "SampleAppPublisher", 
                "SampleAppReceiverA", "2");
            
            InMemorySchemaDataBuilder.ConfigureBinding(
                "SampleAppPublisher", 
                "SampleAppReceiverB", "3");
            
            Orchestrator.Deploy(publisherId);
            Orchestrator.Deploy(receiverAId);
            Orchestrator.Deploy(receiverBId);

            // Act
            Orchestrator.Start(publisherId);
            Orchestrator.Start(receiverAId);
            Orchestrator.Start(receiverBId);

            Task.Run(async () =>
            {
                await Task.Delay(1000);
                await Orchestrator.Stop(receiverAId);
                await Orchestrator.Stop(receiverBId);
            }).Wait();
            
            // Assert
            // var publisherComponent = (IApplicationWithContext) Orchestrator.GetManagedApplicationInstance(publisherId).GetRootComponent();
            // var publisherProperties = publisherComponent.ContextContainer.Resolve<MockPublisherProperties>();
            //
            // var receiverAComponent = (IApplicationWithContext) Orchestrator.GetManagedApplicationInstance(receiverAId).GetRootComponent();;
            // var receiverAProperties = receiverAComponent.ContextContainer.Resolve<MockReceiverProperties>();
            //
            // var receiverBComponent = (IApplicationWithContext) Orchestrator.GetManagedApplicationInstance(receiverBId).GetRootComponent();;
            // var receiverBProperties = receiverBComponent.ContextContainer.Resolve<MockReceiverProperties>();
            //
            // Assert.AreEqual(10, publisherProperties.PublishedMessages.Count);
            //
            // Assert.AreEqual(1, receiverAProperties.ReceivedMessages.Count);
            // Assert.AreEqual(1,receiverBProperties.ReceivedMessages.Count);
            //
            // Assert.AreEqual("TestString-2", receiverAProperties.ReceivedMessages[0]);
            // Assert.AreEqual("TestString-3", receiverBProperties.ReceivedMessages[0]);
        }
        
        [TestMethod]
        public void PublisherRoleShouldNotFailIfNoAnyReceiver()
        {
            // Arrange
            var publisherId = 1;
            InMemorySchemaDataBuilder.CreateApplicationTemplate(
                    "SampleAppPublisherTemplate", 
                    "2019.1.1")
                .CreateApplicationInstance("SampleAppPublisher", publisherId);
            
            var receiverId = 2;
            InMemorySchemaDataBuilder.CreateApplicationTemplate(
                    "SampleAppReceiverTemplate",
                    "2018.1.1")
                .CreateApplicationInstance("SampleAppReceiver", receiverId);
               
            // Act
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
            // var publisherComponent = (IApplicationWithContext) Orchestrator.GetManagedApplicationInstance(publisherId).GetRootComponent();
            // var publisherProperties = publisherComponent.ContextContainer.Resolve<MockPublisherProperties>();
            //
            // var receiverComponent = (IApplicationWithContext) Orchestrator.GetManagedApplicationInstance(receiverId).GetRootComponent();;
            // var receiverProperties = receiverComponent.ContextContainer.Resolve<MockReceiverProperties>();
            //
            // Assert.AreEqual("TestString", publisherProperties.PublishedMessages[0]);
            // Assert.AreEqual(0, receiverProperties.ReceivedMessages.Count);
        }
    }
}