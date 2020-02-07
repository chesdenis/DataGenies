using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataGenies.Core.Behaviours;
using DataGenies.Core.Converters;
using DataGenies.Core.Roles;
using DataGenies.Core.Tests.Integration.Mocks.Abstractions;
using DataGenies.Core.Tests.Integration.Mocks.ApplicationTemplates;
using DataGenies.Core.Tests.Integration.Mocks.Behaviours;
using DataGenies.InMemory;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataGenies.Core.Tests.Integration.Behaviours
{
    [TestClass]
    public class BeforeRunBehaviourTests
    {
        private MqBroker _inMemoryMqBroker;

        private MqConfigurator _mqConfigurator;

        private SchemaDataContext _inMemorySchemaContext;

        private SchemaDataBuilder _schemaDataBuilder;
        
        [TestInitialize]
        public void Initialize()
        {
            _inMemoryMqBroker = new MqBroker();
            
            _inMemorySchemaContext = new SchemaDataContext();
            _schemaDataBuilder = new SchemaDataBuilder(_inMemorySchemaContext);
            _mqConfigurator = new MqConfigurator(_inMemoryMqBroker);
        }
        
        [TestMethod]
        public void SimpleBeforeStartBehaviourShouldHasAccessToComponentPropertiesIfTheyExist()
        {
            // Arrange
            _schemaDataBuilder.CreateApplicationTemplate(
                    "SampleAppPublisherTemplate", 
                    "2019.1.1")
                .CreateApplicationInstance("SampleAppPublisher");
            
            _schemaDataBuilder.CreateApplicationTemplate(
                    "SampleAppReceiverTemplate", 
                    "2018.1.1")
                .CreateApplicationInstance("SampleAppReceiver");

            _schemaDataBuilder.ConfigureBinding(
                "SampleAppPublisher", 
                "SampleAppReceiver", "#");

            var receiverBuilder = new ReceiverBuilder(_inMemoryMqBroker);
            var publisherBuilder = new PublisherBuilder(_inMemoryMqBroker);

            var simpleBehaviour = new MockSimpleBeforeStartBehaviour();
            
            var managedApplicationBuilder =
                new ManagedApplicationBuilder(_inMemorySchemaContext, receiverBuilder, publisherBuilder,
                    _mqConfigurator);
            
            //managedApplicationBuilder.UsingBehaviours(simpleBehaviour);
            
            var publisherManagedApp = managedApplicationBuilder.UsingApplicationInstance(
                    _inMemorySchemaContext.ApplicationInstances
                        .First(f => f.Name == "SampleAppPublisher"))
                .UsingTemplateType(typeof(MockAppTemplateSimplePublisherWithState)).Build();
            
            managedApplicationBuilder =
                new ManagedApplicationBuilder(_inMemorySchemaContext, receiverBuilder, publisherBuilder,
                    _mqConfigurator);
            
            var receiverManagedApp = managedApplicationBuilder.UsingApplicationInstance(
                    _inMemorySchemaContext.ApplicationInstances
                        .First(f => f.Name == "SampleAppReceiver"))
                .UsingTemplateType(typeof(MockAppTemplateSimpleReceiver)).Build();
            
            // Act
            var t1 = Task.Run(() => publisherManagedApp.Start());
            
            var t2 = Task.Run(() =>
            {
                Task.Run(async () =>
                {
                    await Task.Delay(1000);
                    receiverManagedApp.Stop();
                });
                
                receiverManagedApp.Start();
            });
            
            Task.WaitAll(t1, t2);
            
            // Assert
            Assert.IsTrue(simpleBehaviour.SomeData.Count == 2);
            Assert.AreEqual("ABC", simpleBehaviour.SomeData[0]);
            Assert.AreEqual("DEF", simpleBehaviour.SomeData[1]);
        }
    }
}