using System;
using System.Collections.Generic;
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
            
            _schemaDataBuilder.CreateApplicationTemplate(
                    "SampleAppReceiverTemplate",
                    "2018.1.1")
                .CreateApplicationInstance("SampleAppReceiver");
             
            var publisherManagedApp = _managedApplicationBuilder.UsingApplicationInstance(
                    _inMemorySchemaContext.ApplicationInstances
                        .First(f => f.Name == "SampleAppPublisher"))
                .UsingTemplateType(typeof(SampleAppPublisher)).Build();
            
            var receiverManagedApp = _managedApplicationBuilder.UsingApplicationInstance(
                    _inMemorySchemaContext.ApplicationInstances
                        .First(f => f.Name == "SampleAppReceiver"))
                .UsingTemplateType(typeof(SampleAppReceiver)).Build();
            
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
            Assert.AreEqual("TestString", ((SampleAppPublisher) (publisherManagedApp.GetRootComponent())).State[0]);
            Assert.IsTrue(((SampleAppReceiver) (receiverManagedApp.GetRootComponent())).State.Count == 0);
        }
        
        
        [ApplicationTemplate]
        private class SampleAppPublisher : ApplicationPublisherRole
        {
            public readonly List<string> State = new List<string>();
            
            public SampleAppPublisher(DataPublisherRole publisherRole) : base(publisherRole)
            {
            }

            public override void Start()
            {
                var testData = Encoding.UTF8.GetBytes("TestString");

                this.Publish(testData);
                
                State.Add("TestString");
            }

            public override void Stop()
            {
                
            }
        }
        
        [ApplicationTemplate]
        private class SampleAppReceiver : ApplicationReceiverRole
        {
            public readonly List<string> State = new List<string>();
            
            public SampleAppReceiver(DataReceiverRole receiverRole) : base(receiverRole)
            {
            }

            public override void Start()
            {
                this.Listen((message) =>
                {
                    State.Add(Encoding.UTF8.GetString(message));
                });
            }

            public override void Stop()
            {
                this.StopListen();
            }
        }
    }
}