using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataGenies.Core.Attributes;
using DataGenies.Core.Behaviours;
using DataGenies.Core.Converters;
using DataGenies.Core.Roles;
using DataGenies.Core.Tests.Integration.Mocks;
using DataGenies.Core.Tests.Integration.Mocks.ApplicationTemplates;
using DataGenies.InMemory;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataGenies.Core.Tests.Integration.Roles
{
    [TestClass]
    public class ApplicationReceiverAndPublisherRoleTests
    {
        private MqBroker _inMemoryMqBroker;

        private MqConfigurator _mqConfigurator;
        
        private SchemaDataContext _inMemorySchemaContext;

        private SchemaDataBuilder _schemaDataBuilder;

        private ManagedApplicationBuilder _managedApplicationBuilder;
        
        
        [TestInitialize]
        public void Initialize()
        {
            _inMemoryMqBroker = new MqBroker();
            
            var receiverBuilder = new ReceiverBuilder(_inMemoryMqBroker);
            var publisherBuilder = new PublisherBuilder(_inMemoryMqBroker);
            
            _inMemorySchemaContext = new SchemaDataContext();
            _schemaDataBuilder = new SchemaDataBuilder(_inMemorySchemaContext);
            _mqConfigurator = new MqConfigurator(_inMemoryMqBroker);

            _managedApplicationBuilder =
                new ManagedApplicationBuilder(_inMemorySchemaContext, receiverBuilder, publisherBuilder,
                    _mqConfigurator);
        }

        [TestMethod]
        public void ReceiverAndPublisherRoleShouldReceiveMessagesAndPushThey()
        {
            // Arrange
            _schemaDataBuilder.CreateApplicationTemplate(
                    "SampleAppPublisherTemplate",
                    "2019.1.1")
                .CreateApplicationInstance("SampleAppPublisher");
            
            _schemaDataBuilder.CreateApplicationTemplate(
                    "SampleAppReceiverAndPublisherTemplate",
                    "2019.1.1")
                .CreateApplicationInstance("SampleAppReceiverAndPublisher");
            
            _schemaDataBuilder.CreateApplicationTemplate(
                    "SampleAppReceiverTemplate",
                    "2018.1.1")
                .CreateApplicationInstance("SampleAppReceiver");
            
            _schemaDataBuilder.ConfigureBinding(
                "SampleAppPublisher",
                "SampleAppReceiverAndPublisher", "#");
            
            _schemaDataBuilder.ConfigureBinding(
                "SampleAppReceiverAndPublisher",
                "SampleAppReceiver", "#");
            
            var publisherManagedApp = _managedApplicationBuilder.UsingApplicationInstance(
                    _inMemorySchemaContext.ApplicationInstances
                        .First(f => f.Name == "SampleAppPublisher"))
                .UsingTemplateType(typeof(MockAppTemplateSimplePublisher)).Build();
            
            var receiverAndPublisherManagedApp = _managedApplicationBuilder.UsingApplicationInstance(
                    _inMemorySchemaContext.ApplicationInstances
                        .First(f => f.Name == "SampleAppReceiverAndPublisher"))
                .UsingTemplateType(typeof(MockAppTemplateSimpleReceiverAndPublisher)).Build();
            
            var receiverManagedApp = _managedApplicationBuilder.UsingApplicationInstance(
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
                    receiverAndPublisherManagedApp.Stop();
                });
            
                receiverAndPublisherManagedApp.Start();
            });
            
            var t3 = Task.Run(() =>
            {
                Task.Run(async () =>
                {
                    await Task.Delay(1000);
                    receiverManagedApp.Stop();
                });
            
                receiverManagedApp.Start();
            });
            
            // Assert
            Task.WaitAll(t1, t2, t3);
            
            Assert.AreEqual("TestString", ((MockAppTemplateSimplePublisher) (publisherManagedApp.GetRootComponent())).GetLastMessageAsString());
            Assert.AreEqual("TestString-changed!", ((MockAppTemplateSimpleReceiverAndPublisher) (receiverAndPublisherManagedApp.GetRootComponent())).GetLastMessageAsString());
            Assert.AreEqual("TestString-changed!", ((MockAppTemplateSimpleReceiver) (receiverManagedApp.GetRootComponent())).GetLastMessageAsString());
        }
    }
}