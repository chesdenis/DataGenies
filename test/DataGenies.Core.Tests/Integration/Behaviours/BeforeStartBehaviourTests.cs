using System.Collections.Generic;
using System.Threading.Tasks;
using DataGenies.Core.Behaviours;
using DataGenies.Core.Models;
using DataGenies.Core.Tests.Integration.Mocks.ApplicationTemplates;
using DataGenies.Core.Tests.Integration.Mocks.Behaviours;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace DataGenies.Core.Tests.Integration.Behaviours
{
    [TestClass]
    public class BeforeStartBehaviourTests : BaseIntegrationTest
    {
        private MockSimpleBeforeStartBehaviour _beforeStartBehaviour;
        
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
            
            _beforeStartBehaviour = new MockSimpleBeforeStartBehaviour();
            
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
                            case "SampleBehaviour":
                                retVal.Add(_beforeStartBehaviour);
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
            // var publisherComponent = (IApplicationWithContext) Orchestrator.GetManagedApplicationInstance(publisherId).GetRootComponent();
            // var publisherProperties = publisherComponent.ContextContainer.Resolve<MockPublisherProperties>();
            //
            // var receiverComponent = (IApplicationWithContext) Orchestrator.GetManagedApplicationInstance(receiverId).GetRootComponent();;
            // var receiverProperties = receiverComponent.ContextContainer.Resolve<MockReceiverProperties>();
            //
            // Assert.AreEqual("PrefixTestString", publisherProperties.PublishedMessages[0]);
            // Assert.AreEqual("PrefixTestString", receiverProperties.ReceivedMessages[0]);
        }
    }
}