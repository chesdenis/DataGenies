using System.Threading.Tasks;
using DataGenies.Core.Models;
using DataGenies.Core.Tests.Integration.Mocks.ApplicationTemplates;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace DataGenies.Core.Tests.Integration.Roles
{
    [TestClass]
    public class ApplicationReceiverRoleTests: BaseIntegrationTest
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
                    Arg.Is<ApplicationTemplateEntity>(w => w.Name == "SampleAppBrokenReceiverTemplate"))
                .Returns(typeof(MockBrokenReceiver));
        }
         
         [TestMethod]
        public void BrokenReceiverRoleShouldNotAckMessages()
        {
            // Arrange
            var publisherId = 1;
            InMemorySchemaDataBuilder.CreateApplicationTemplate(
                    "SampleAppPublisherTemplate", 
                    "2019.1.1")
                .CreateApplicationInstance("SampleAppPublisher", publisherId);
            
            var receiverId = 2;
            InMemorySchemaDataBuilder.CreateApplicationTemplate(
                    "SampleAppBrokenReceiverTemplate", 
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
            // var publisherComponent = (IApplicationWithContext) Orchestrator.GetManagedApplicationInstance(publisherId).GetRootComponent();
            // var publisherProperties = publisherComponent.ContextContainer.Resolve<MockPublisherProperties>();
            //
            // var receiverComponent = (IApplicationWithContext) Orchestrator.GetManagedApplicationInstance(receiverId).GetRootComponent();;
            // var receiverProperties = receiverComponent.ContextContainer.Resolve<MockReceiverProperties>();
            //
            // Assert.AreEqual("TestString", publisherProperties.PublishedMessages[0]);
            // Assert.AreEqual(0, receiverProperties.ReceivedMessages.Count);
        }
        
        [TestMethod]
        public void ReceiverRoleShouldReceiveMessagesFromPublisher()
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
    }
}