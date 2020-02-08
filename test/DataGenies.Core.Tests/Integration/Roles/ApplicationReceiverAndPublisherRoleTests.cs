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
    public class ApplicationReceiverAndPublisherRoleTests : BaseIntegrationTest
    {
       
        [TestInitialize]
        public override void Initialize()
        { 
            base.Initialize();
            
            _applicationTemplatesScanner.FindType(
                    Arg.Is<ApplicationTemplateEntity>(w => w.Name == "SampleAppPublisherTemplate"))
                .Returns(typeof(MockSimplePublisher));
            
            _applicationTemplatesScanner.FindType(
                    Arg.Is<ApplicationTemplateEntity>(w => w.Name == "SampleAppReceiverAndPublisherTemplate"))
                .Returns(typeof(MockSimpleReceiverAndPublisher));
            
            _applicationTemplatesScanner.FindType(
                    Arg.Is<ApplicationTemplateEntity>(w => w.Name == "SampleAppReceiverTemplate"))
                .Returns(typeof(MockSimpleReceiver));
           
        }

        [TestMethod]
        public void ReceiverAndPublisherRoleShouldReceiveMessagesAndPushThey()
        {
            // Arrange
            var publisherId = 1;
            InMemorySchemaDataBuilder.CreateApplicationTemplate(
                    "SampleAppPublisherTemplate",
                    "2019.1.1")
                .CreateApplicationInstance("SampleAppPublisher", publisherId);

            var mixedRoleId = 2;
            InMemorySchemaDataBuilder.CreateApplicationTemplate(
                    "SampleAppReceiverAndPublisherTemplate",
                    "2019.1.1")
                .CreateApplicationInstance("SampleAppReceiverAndPublisher", mixedRoleId);

            var receiverId = 3;
            InMemorySchemaDataBuilder.CreateApplicationTemplate(
                    "SampleAppReceiverTemplate",
                    "2018.1.1")
                .CreateApplicationInstance("SampleAppReceiver", receiverId);
            
            InMemorySchemaDataBuilder.ConfigureBinding(
                "SampleAppPublisher",
                "SampleAppReceiverAndPublisher", "#");
            
            InMemorySchemaDataBuilder.ConfigureBinding(
                "SampleAppReceiverAndPublisher",
                "SampleAppReceiver", "#");
            
            _orchestrator.Deploy(publisherId);
            _orchestrator.Deploy(mixedRoleId);
            _orchestrator.Deploy(receiverId);
           
            
            // Act
            _orchestrator.Start(publisherId);
            _orchestrator.Start(mixedRoleId);
            _orchestrator.Start(receiverId);
            
            Task.Run(async () =>
            {
                await Task.Delay(1000);
                await _orchestrator.Stop(mixedRoleId);
                await _orchestrator.Stop(receiverId);
            }).Wait();
            
            // Assert
            var publisherComponent = (IApplicationWithContext) _orchestrator.GetManagedApplicationInstance(publisherId).GetRootComponent();
            var publisherProperties = publisherComponent.ContextContainer.Resolve<MockPublisherProperties>();
            
            var mixedRoleComponent = (IApplicationWithContext) _orchestrator.GetManagedApplicationInstance(mixedRoleId).GetRootComponent();
            var mixedRolePublisherProperties = mixedRoleComponent.ContextContainer.Resolve<MockPublisherProperties>();
            var mixedRoleReceiverProperties = mixedRoleComponent.ContextContainer.Resolve<MockReceiverProperties>();
            
            var receiverComponent = (IApplicationWithContext) _orchestrator.GetManagedApplicationInstance(receiverId).GetRootComponent();
            var receiverProperties = receiverComponent.ContextContainer.Resolve<MockReceiverProperties>();
 
            Assert.AreEqual("TestString", publisherProperties.PublishedMessages[0]);
            
            Assert.AreEqual("TestString", mixedRoleReceiverProperties.ReceivedMessages[0]);
            Assert.AreEqual("TestString-changed!", mixedRolePublisherProperties.PublishedMessages[0]);
            
            Assert.AreEqual("TestString-changed!", receiverProperties.ReceivedMessages[0]);
        }
    }
}