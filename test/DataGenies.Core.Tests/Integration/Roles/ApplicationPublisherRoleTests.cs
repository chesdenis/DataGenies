using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataGenies.Core.Attributes;
using DataGenies.Core.Roles;
using DataGenies.InMemory;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataGenies.Core.Tests.Integration.Roles
{
    [TestClass]
    public class ApplicationPublisherRoleTests
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
                new ManagedApplicationBuilder(_inMemorySchemaContext, receiverBuilder, publisherBuilder, _mqConfigurator);
        }

        [TestMethod]
        public void PublisherRoleShouldDeliverMessagesToOneReceiver()
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
            
            // Act
            var appInfo = this._inMemorySchemaContext.ApplicationInstances.First();
            var managedApp = _managedApplicationBuilder.UsingApplicationInstance(appInfo)
                .UsingTemplateType(typeof(SampleAppPublisher)).Build();

            managedApp.Start();
            
            // Assert
            var queues = _inMemoryMqBroker.Model["SampleAppPublisher"]["#"];
            Assert.AreEqual(1, queues.Count);

            var queue = queues.First();
            Assert.AreEqual(1, queue.Count);
            queue.TryDequeue(out MqMessage message);
            Assert.AreEqual("#", message.RoutingKey);
            Assert.AreEqual("TestString",  Encoding.UTF8.GetString(message.Body));
        }
        
        [TestMethod]
        public void PublisherRoleShouldDeliverMessagesToMultipleReceivers()
        {
            // Arrange
            _schemaDataBuilder.CreateApplicationTemplate(
                    "SampleAppPublisherTemplate", 
                    "2019.1.1")
                .CreateApplicationInstance("SampleAppPublisher");
            
            _schemaDataBuilder.CreateApplicationTemplate(
                    "SampleAppReceiverTemplate", 
                    "2018.1.1")
                .CreateApplicationInstance("SampleAppReceiverA");
            
            _schemaDataBuilder.CreateApplicationTemplate(
                    "SampleAppReceiverTemplate", 
                    "2018.1.1")
                .CreateApplicationInstance("SampleAppReceiverB");

            _schemaDataBuilder.ConfigureBinding(
                "SampleAppPublisher", 
                "SampleAppReceiverA", "#");
            
            _schemaDataBuilder.ConfigureBinding(
                "SampleAppPublisher", 
                "SampleAppReceiverB", "#");
            
            // Act
            var appInfo = this._inMemorySchemaContext.ApplicationInstances.First();
            var managedApp = _managedApplicationBuilder.UsingApplicationInstance(appInfo)
                .UsingTemplateType(typeof(SampleAppPublisher)).Build();

            managedApp.Start();
            
            // Assert
            var queues = _inMemoryMqBroker.Model["SampleAppPublisher"]["#"];
            Assert.AreEqual(2, queues.Count);
            
            Assert.IsTrue(queues.All(w => w.Count == 1));
             
            foreach (var queue in queues)
            {
                queue.TryDequeue(out MqMessage message);
                Assert.AreEqual("#", message.RoutingKey);
                Assert.AreEqual("TestString",  Encoding.UTF8.GetString(message.Body));
            }
        }
        
        [TestMethod]
        public void PublisherRoleShouldNotFailIfNoAnyReceiver()
        {
            // Arrange
            _schemaDataBuilder.CreateApplicationTemplate(
                    "SampleAppPublisherTemplate", 
                    "2019.1.1")
                .CreateApplicationInstance("SampleAppPublisher");
            
            // Act
            var appInfo = this._inMemorySchemaContext.ApplicationInstances.First();
            var managedApp = _managedApplicationBuilder.UsingApplicationInstance(appInfo)
                .UsingTemplateType(typeof(SampleAppPublisher)).Build();

            managedApp.Start();
            
            // Assert
            Assert.IsTrue(!_inMemoryMqBroker.Model["SampleAppPublisher"].ContainsKey("#"));
        }
        
        
        [ApplicationTemplate]
        private class SampleAppPublisher : ApplicationPublisherRole
        {
            public SampleAppPublisher(DataPublisherRole publisherRole) : base(publisherRole)
            {
            }

            public override void Start()
            {
                var testData = Encoding.UTF8.GetBytes("TestString");

                this.Publish(testData);
            }

            public override void Stop()
            {
                
            }
        }
    }
}