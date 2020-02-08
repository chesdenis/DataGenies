using System.Threading.Tasks;
using DataGenies.Core.Models;
using DataGenies.Core.Roles;
using DataGenies.Core.Tests.Integration.Mocks.ApplicationTemplates;
using DataGenies.Core.Tests.Integration.Mocks.Properties;
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
            
            _applicationTemplatesScanner.FindType(
                    Arg.Is<ApplicationTemplateEntity>(w => w.Name == "SampleAppPublisherTemplate"))
                .Returns(typeof(MockSimplePublisher));
            _applicationTemplatesScanner.FindType(
                    Arg.Is<ApplicationTemplateEntity>(w => w.Name == "SampleAppReceiverTemplate"))
                .Returns(typeof(MockSimpleReceiver));
            _applicationTemplatesScanner.FindType(
                    Arg.Is<ApplicationTemplateEntity>(w => w.Name == "SampleAppBrokenReceiverTemplate"))
                .Returns(typeof(MockBrokenReceiver));
        }
         
         [TestMethod]
        public void BrokenReceiverRoleShouldNotAckMessages()
        {
            // Arrange
            var publisherId = 1;
            _schemaDataBuilder.CreateApplicationTemplate(
                    "SampleAppPublisherTemplate", 
                    "2019.1.1")
                .CreateApplicationInstance("SampleAppPublisher", publisherId);
            
            var receiverId = 2;
            _schemaDataBuilder.CreateApplicationTemplate(
                    "SampleAppBrokenReceiverTemplate", 
                    "2018.1.1")
                .CreateApplicationInstance("SampleAppReceiver", receiverId);
            
            _schemaDataBuilder.ConfigureBinding(
                "SampleAppPublisher", 
                "SampleAppReceiver", "#");

            _orchestrator.Deploy(publisherId);
            _orchestrator.Deploy(receiverId);
             
            // Act
            _orchestrator.Start(publisherId);
            _orchestrator.Start(receiverId);
            
            Task.Run(async () =>
            {
                await Task.Delay(1000);
                await _orchestrator.Stop(receiverId);
            }).Wait();
            
            // Assert
            var publisherComponent = (IApplicationWithContext) _orchestrator.GetManagedApplicationInstance(publisherId).GetRootComponent();
            var publisherProperties = publisherComponent.ContextContainer.Resolve<MockPublisherProperties>();
            
            var receiverComponent = (IApplicationWithContext) _orchestrator.GetManagedApplicationInstance(receiverId).GetRootComponent();;
            var receiverProperties = receiverComponent.ContextContainer.Resolve<MockReceiverProperties>();
 
            Assert.AreEqual("TestString", publisherProperties.PublishedMessages[0]);
            Assert.AreEqual(0, receiverProperties.ReceivedMessages.Count);
        }
        
        [TestMethod]
        public void ReceiverRoleShouldReceiveMessagesFromPublisher()
        {
            // Arrange
            var publisherId = 1;
            _schemaDataBuilder.CreateApplicationTemplate(
                    "SampleAppPublisherTemplate", 
                    "2019.1.1")
                .CreateApplicationInstance("SampleAppPublisher", publisherId);
            
            var receiverId = 2;
            _schemaDataBuilder.CreateApplicationTemplate(
                    "SampleAppReceiverTemplate", 
                    "2018.1.1")
                .CreateApplicationInstance("SampleAppReceiver", receiverId);
            
            _schemaDataBuilder.ConfigureBinding(
                "SampleAppPublisher", 
                "SampleAppReceiver", "#");
             
            _orchestrator.Deploy(publisherId);
            _orchestrator.Deploy(receiverId);
             
            // Act
            _orchestrator.Start(publisherId);
            _orchestrator.Start(receiverId);
            
            Task.Run(async () =>
            {
                await Task.Delay(1000);
                await _orchestrator.Stop(receiverId);
            }).Wait();
             
            // Assert
            var publisherComponent = (IApplicationWithContext) _orchestrator.GetManagedApplicationInstance(publisherId).GetRootComponent();
            var publisherProperties = publisherComponent.ContextContainer.Resolve<MockPublisherProperties>();
            
            var receiverComponent = (IApplicationWithContext) _orchestrator.GetManagedApplicationInstance(receiverId).GetRootComponent();;
            var receiverProperties = receiverComponent.ContextContainer.Resolve<MockReceiverProperties>();

            Assert.AreEqual("TestString", publisherProperties.PublishedMessages[0]);
            Assert.AreEqual("TestString", receiverProperties.ReceivedMessages[0]);
        }
    }
}