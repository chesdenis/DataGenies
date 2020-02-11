using System.Collections.Generic;
using System.Threading.Tasks;
using DataGenies.Core.Behaviours;
using DataGenies.Core.Models;
using DataGenies.Core.Roles;
using DataGenies.Core.Tests.Integration.Mocks.ApplicationTemplates;
using DataGenies.Core.Tests.Integration.Mocks.Behaviours;
using DataGenies.Core.Tests.Integration.Mocks.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace DataGenies.Core.Tests.Integration.Behaviours
{
    [TestClass]
    public class HandleErroredMessagesBehaviorTests : BaseIntegrationTest
    {
        private MockHandleErroredMessagesBehavior _handleErroredMessagesBehavior;

        [TestInitialize]
        public override void Initialize()
        {
            base.Initialize();
            
            ApplicationTemplatesScanner.FindType(
                    Arg.Is<ApplicationTemplateEntity>(w => w.Name == "MockSimplePublisher"))
                .Returns(typeof(MockSimplePublisher));
            ApplicationTemplatesScanner.FindType(
                    Arg.Is<ApplicationTemplateEntity>(w => w.Name == "MockBrokenReceiverAndPublisher"))
                .Returns(typeof(MockBrokenReceiverAndPublisher));
            ApplicationTemplatesScanner.FindType(
                    Arg.Is<ApplicationTemplateEntity>(w => w.Name == "MockErrorCollectorReceiver"))
                .Returns(typeof(MockSimpleReceiver));
            
            _handleErroredMessagesBehavior = new MockHandleErroredMessagesBehavior();

            ApplicationBehavioursScanner.GetBehavioursInstances(
                    Arg.Any<IEnumerable<BehaviourEntity>>())
                .Returns((cb) =>
                {
                    var retVal = new List<IBehaviour>();
                    var behavioursEntities = cb.Arg<IEnumerable<BehaviourEntity>>();
            
                    foreach (var behaviourEntity in behavioursEntities)
                    {
                        switch (behaviourEntity.Name)
                        {
                            case "HandleErroredMessagesBehavior":
                                retVal.Add(_handleErroredMessagesBehavior);
                                break;
                        }
                    }
                    
                    return retVal;
                });
        }

        [TestMethod]
        public void SimpleBeforeStartBehaviourShouldHasAccessToComponentPropertiesIfTheyExist()
        {
            // Arrange
            var publisherId = 1;
            InMemorySchemaDataBuilder.CreateApplicationTemplate(
                    "MockSimplePublisher",
                    "2019.1.1")
                .CreateApplicationInstance("MockSimplePublisherInstance", publisherId);
            
            var receiverAId = 2;
            InMemorySchemaDataBuilder.CreateApplicationTemplate(
                    "MockBrokenReceiverAndPublisher",
                    "2018.1.1")
                .CreateApplicationInstance("MockBrokenReceiverAndPublisherInstance", receiverAId)
                .RegisterBehaviour("HandleErroredMessagesBehavior", "2019.1.1")
                .ApplyBehaviour();

            var receiverBId = 3;
            InMemorySchemaDataBuilder.CreateApplicationTemplate(
                    "MockErrorCollectorReceiver",
                    "2018.1.1")
                .CreateApplicationInstance("MockSimpleErrorCollectorReceiverInstance", receiverBId);

            InMemorySchemaDataBuilder.ConfigureBinding(
                "MockSimplePublisherInstance",
                "MockBrokenReceiverAndPublisherInstance", "#");
            InMemorySchemaDataBuilder.ConfigureBinding(
                "MockBrokenReceiverAndPublisherInstance",
                "MockSimpleErrorCollectorReceiverInstance", "Errors");

            Orchestrator.Deploy(publisherId);
            Orchestrator.Deploy(receiverAId);
            Orchestrator.Deploy(receiverBId);

            // Act
            Orchestrator.Start(publisherId);
            Orchestrator.Start(receiverAId);
            //Orchestrator.Start(receiverBId);

            Task.Run(async () =>
            {
                await Task.Delay(1000);
                await Orchestrator.Stop(receiverAId);
                //await Orchestrator.Stop(receiverBId);
            }).Wait();
            
            // Assert
            var publisherComponent = (IApplicationWithContext) Orchestrator.GetManagedApplicationInstance(publisherId).GetRootComponent();
            var publisherProperties = publisherComponent.ContextContainer.Resolve<MockPublisherProperties>();
            
            var receiverAComponent = (IApplicationWithContext) Orchestrator.GetManagedApplicationInstance(receiverAId).GetRootComponent();;
            var receiverAProperties = receiverAComponent.ContextContainer.Resolve<MockReceiverProperties>();
            
            var receiverBComponent = (IApplicationWithContext) Orchestrator.GetManagedApplicationInstance(receiverBId).GetRootComponent();;
            var receiverBProperties = receiverBComponent.ContextContainer.Resolve<MockReceiverProperties>();

            
            Assert.AreEqual("TestString", publisherProperties.PublishedMessages[0]);
            Assert.AreEqual(0, receiverAProperties.ReceivedMessages.Count);
            //Assert.AreEqual(1, receiverBProperties.ReceivedMessages.Count);

        }
    }
}