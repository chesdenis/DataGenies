using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataGenies.Core.Models;
using DataGenies.Core.Tests.Integration.Mocks.ApplicationTemplates;
using DataGenies.Core.Tests.Integration.Mocks.Converters;
using DataGenies.Core.Tests.Integration.Mocks.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace DataGenies.Core.Tests.Integration.Converters
{
    [TestClass]
    public class ConverterTests : BaseIntegrationTest
    {
        [TestInitialize]
        public override void Initialize()
        {
            base.Initialize();
            
            // ApplicationTemplatesScanner.FindType(
            //         Arg.Is<ApplicationTemplateEntity>(w => w.Name == "SampleAppPublisherTemplate"))
            //     .Returns(typeof(MockSimplePublisher));
            //
            // ApplicationTemplatesScanner.FindType(
            //         Arg.Is<ApplicationTemplateEntity>(w => w.Name == "SampleAppReceiverAndPublisherTemplate"))
            //     .Returns(typeof(MockSimpleReceiverAndPublisher));
            //
            // ApplicationTemplatesScanner.FindType(
            //         Arg.Is<ApplicationTemplateEntity>(w => w.Name == "SampleAppReceiverTemplate"))
            //     .Returns(typeof(MockSimpleReceiver));
            //
            // ApplicationConvertersScanner.GetConvertersInstances( Arg.Any<IEnumerable<ConverterEntity>>())
            //     .Returns((cb) =>
            //     {
            //         var retVal = new List<IConverter>();
            //         var convertersEntities = cb.Arg<IEnumerable<ConverterEntity>>();
            //
            //         foreach (var converterEntity in convertersEntities)
            //         {
            //             switch (converterEntity.Name)
            //             {
            //                 case "SampleRevertBeforePublishConverter":
            //                     retVal.Add(new MockRevertTextBeforePublishConverter());
            //                     break;
            //                 case "SampleRevertAfterReceiveConverter":
            //                     retVal.Add(new MockRevertTextAfterReceiveConverter());
            //                     break;
            //             }
            //         }
            //         
            //         return retVal;
            //     });
        }

        [TestMethod]
        public void ShouldApplyConvertersBeforeAndAfterPublish()
        {
            //  // Arrange
            // var publisherId = 1;
            // InMemorySchemaDataBuilder.CreateApplicationTemplate(
            //         "SampleAppPublisherTemplate",
            //         "2019.1.1")
            //     .CreateApplicationInstance("SampleAppPublisher", publisherId)
            //     .RegisterConverter("SampleRevertBeforePublishConverter", "2019.1.1")
            //     .ApplyConverter();
            //
            // var mixedRoleId = 2;
            // InMemorySchemaDataBuilder.CreateApplicationTemplate(
            //         "SampleAppReceiverAndPublisherTemplate",
            //         "2019.1.1")
            //     .CreateApplicationInstance("SampleAppReceiverAndPublisher", mixedRoleId);
            //
            // var receiverId = 3;
            // InMemorySchemaDataBuilder.CreateApplicationTemplate(
            //         "SampleAppReceiverTemplate",
            //         "2018.1.1")
            //     .CreateApplicationInstance("SampleAppReceiver", receiverId)
            //     .RegisterConverter("SampleRevertAfterReceiveConverter", "2019.1.1")
            //     .ApplyConverter();
            //
            // InMemorySchemaDataBuilder.ConfigureBinding(
            //     "SampleAppPublisher",
            //     "SampleAppReceiverAndPublisher", "#");
            //
            // InMemorySchemaDataBuilder.ConfigureBinding(
            //     "SampleAppReceiverAndPublisher",
            //     "SampleAppReceiver", "#");
            //
            // Orchestrator.Deploy(publisherId);
            // Orchestrator.Deploy(mixedRoleId);
            // Orchestrator.Deploy(receiverId);
            //
            //
            // // Act
            // Orchestrator.Start(publisherId);
            // Orchestrator.Start(mixedRoleId);
            // Orchestrator.Start(receiverId);
            //
            // Task.Run(async () =>
            // {
            //     await Task.Delay(1000);
            //     await Orchestrator.Stop(mixedRoleId);
            //     await Orchestrator.Stop(receiverId);
            // }).Wait();
            
            // Assert
            // var publisherComponent = (IApplicationWithContext) Orchestrator.GetManagedApplicationInstance(publisherId).GetRootComponent();
            // var publisherProperties = publisherComponent.ContextContainer.Resolve<MockPublisherProperties>();
            //
            // var mixedRoleComponent = (IApplicationWithContext) Orchestrator.GetManagedApplicationInstance(mixedRoleId).GetRootComponent();
            // var mixedRolePublisherProperties = mixedRoleComponent.ContextContainer.Resolve<MockPublisherProperties>();
            // var mixedRoleReceiverProperties = mixedRoleComponent.ContextContainer.Resolve<MockReceiverProperties>();
            //
            // var receiverComponent = (IApplicationWithContext) Orchestrator.GetManagedApplicationInstance(receiverId).GetRootComponent();
            // var receiverProperties = receiverComponent.ContextContainer.Resolve<MockReceiverProperties>();
            //
            // Assert.AreEqual("TestString", publisherProperties.PublishedMessages[0]);
            //
            // Assert.AreEqual(new string("TestString".Reverse().ToArray()), mixedRoleReceiverProperties.ReceivedMessages[0]);
            // Assert.AreEqual($"{new string("TestString".Reverse().ToArray())}-changed!", mixedRolePublisherProperties.PublishedMessages[0]);
            //
            // Assert.AreEqual(new string($"{new string("TestString".Reverse().ToArray())}-changed!".Reverse().ToArray()), receiverProperties.ReceivedMessages[0]);
        }
    }
}