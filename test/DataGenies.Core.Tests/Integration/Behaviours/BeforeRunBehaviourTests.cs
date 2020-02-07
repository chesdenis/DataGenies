using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataGenies.Core.Behaviours;
using DataGenies.Core.Converters;
using DataGenies.Core.Models;
using DataGenies.Core.Orchestrators;
using DataGenies.Core.Roles;
using DataGenies.Core.Scanners;
using DataGenies.Core.Tests.Integration.Mocks.Abstractions;
using DataGenies.Core.Tests.Integration.Mocks.ApplicationTemplates;
using DataGenies.Core.Tests.Integration.Mocks.Behaviours;
using DataGenies.InMemory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace DataGenies.Core.Tests.Integration.Behaviours
{
    [TestClass]
    public class BeforeRunBehaviourTests
    {
        private MqBroker _inMemoryMqBroker;

        private MqConfigurator _mqConfigurator;

        private SchemaDataContext _inMemorySchemaContext;

        private SchemaDataBuilder _schemaDataBuilder;

        private IOrchestrator _orchestrator;

        private IApplicationTemplatesScanner _applicationTemplatesScanner;

        private IApplicationBehavioursScanner _applicationBehavioursScanner;

        [TestInitialize]
        public void Initialize()
        {
            _inMemoryMqBroker = new MqBroker();
            
            _inMemorySchemaContext = new SchemaDataContext();
            _schemaDataBuilder = new SchemaDataBuilder(_inMemorySchemaContext);
            _mqConfigurator = new MqConfigurator(_inMemoryMqBroker);

            _applicationTemplatesScanner = Substitute.For<IApplicationTemplatesScanner>();
            _applicationBehavioursScanner = Substitute.For<IApplicationBehavioursScanner>();
            
            _orchestrator = new InMemoryOrchestrator(_inMemorySchemaContext,
                _applicationTemplatesScanner,
                _applicationBehavioursScanner,
                new ManagedApplicationBuilder(_inMemorySchemaContext, new ReceiverBuilder(_inMemoryMqBroker),
                    new PublisherBuilder(_inMemoryMqBroker), _mqConfigurator));
        }
        
        [TestMethod]
        public void SimpleBeforeStartBehaviourShouldHasAccessToComponentPropertiesIfTheyExist()
        {
            // Arrange
            var publisherId = 1;
            _schemaDataBuilder.CreateApplicationTemplate(
                    "SampleAppPublisherTemplate", 
                    "2019.1.1", publisherId)
                .CreateApplicationInstance("SampleAppPublisher")
                .RegisterBehaviour("SampleBehaviour", "2019.1.1")
                .ApplyBehaviour();

            var receiverId = 2;
            _schemaDataBuilder.CreateApplicationTemplate(
                    "SampleAppReceiverTemplate",
                    "2018.1.1", receiverId)
                .CreateApplicationInstance("SampleAppReceiver");
            
            _schemaDataBuilder.ConfigureBinding(
                "SampleAppPublisher", 
                "SampleAppReceiver", "#");

            _applicationTemplatesScanner.FindType(
                    Arg.Is<ApplicationTemplate>(w => w.Name == "SampleAppPublisherTemplate"))
                .Returns(typeof(MockAppTemplateSimplePublisherWithState));
            _applicationTemplatesScanner.FindType(
                    Arg.Is<ApplicationTemplate>(w => w.Name == "SampleAppReceiverTemplate"))
                .Returns(typeof(MockAppTemplateSimpleReceiver));

            var sampleBehaviour = new MockSimpleBeforeStartBehaviour();

            _applicationBehavioursScanner.GetBehavioursInstances(
                    Arg.Is<IEnumerable<Behaviour>>(w=>
                        w.Any(ww=>ww.Name == "SampleBehaviour")))
                .Returns(new List<IBehaviour>()
                {
                    sampleBehaviour
                });

            _orchestrator.Deploy(publisherId);
            _orchestrator.Deploy(receiverId);

            // Act
            var t1 = Task.Run(() => _orchestrator.Start(publisherId));
            
            var t2 = Task.Run(() =>
            {
                Task.Run(async () =>
                {
                    await Task.Delay(1000);
                    await _orchestrator.Stop(receiverId);
                });
                
                _orchestrator.Start(receiverId);
            });
            
            Task.WaitAll(t1, t2);
            
            // Assert
            Assert.IsTrue(sampleBehaviour.SomeData.Count == 2);
            Assert.AreEqual("ABC", sampleBehaviour.SomeData[0]);
            Assert.AreEqual("DEF", sampleBehaviour.SomeData[1]);
        }
    }
}