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
    public class ApplicationReceiverRoleTests
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
        public void BrokenReceiverRoleShouldNotAckMessages()
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
            
            var publisherManagedApp = _managedApplicationBuilder.UsingApplicationInstance(
                    _inMemorySchemaContext.ApplicationInstances
                        .First(f => f.Name == "SampleAppPublisher"))
                .UsingTemplateType(typeof(SampleAppPublisher)).Build();
            
            var receiverManagedApp = _managedApplicationBuilder.UsingApplicationInstance(
                    _inMemorySchemaContext.ApplicationInstances
                        .First(f => f.Name == "SampleAppReceiver"))
                .UsingTemplateType(typeof(SampleBrokenAppReceiver)).Build();
            
            // Act
            var t1 = new Task(() => publisherManagedApp.Start());
            
            var t2 = new Task(() =>
            {
                Task.Run(async () =>
                {
                    await Task.Delay(1000);
                    receiverManagedApp.Stop();
                });
                
                receiverManagedApp.Start();
            });
            
            // Assert
            t1.Start();
            t1.Wait();
            {
                var queues = _inMemoryMqBroker.Model["SampleAppPublisher"]["#"];
                Assert.AreEqual(1, queues.Count);
            
                var queue = queues.First();
                Assert.AreEqual(1, queue.Count);
            }
            
            t2.Start();
            t2.Wait();
            {
                var queues = _inMemoryMqBroker.Model["SampleAppPublisher"]["#"];
                Assert.AreEqual(1, queues.Count);
            
                var queue = queues.First();
                Assert.AreEqual(1, queue.Count);
            }
        }
        
        [TestMethod]
        public void ReceiverRoleShouldReceiveMessagesFromPublisher()
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
            
            var publisherManagedApp = _managedApplicationBuilder.UsingApplicationInstance(
                    _inMemorySchemaContext.ApplicationInstances
                        .First(f => f.Name == "SampleAppPublisher"))
                .UsingTemplateType(typeof(SampleAppPublisher)).Build();
            
            var receiverManagedApp = _managedApplicationBuilder.UsingApplicationInstance(
                    _inMemorySchemaContext.ApplicationInstances
                        .First(f => f.Name == "SampleAppReceiver"))
                .UsingTemplateType(typeof(SampleAppReceiver)).Build();
            
            // Act
            var t1 = new Task(() => publisherManagedApp.Start());
            
            var t2 = new Task(() =>
            {
                Task.Run(async () =>
                {
                    await Task.Delay(1000);
                    receiverManagedApp.Stop();
                });
                
                receiverManagedApp.Start();
            });
            
            // Assert
            t1.Start();
            t1.Wait();
            {
                var queues = _inMemoryMqBroker.Model["SampleAppPublisher"]["#"];
                Assert.AreEqual(1, queues.Count);
            
                var queue = queues.First();
                Assert.AreEqual(1, queue.Count);
            }
            
            t2.Start();
            t2.Wait();
            {
                var queues = _inMemoryMqBroker.Model["SampleAppPublisher"]["#"];
                Assert.AreEqual(1, queues.Count);
            
                var queue = queues.First();
                Assert.AreEqual(0, queue.Count);
            }
        }
        
        [ApplicationTemplate]
        private class SampleAppPublisher : ApplicationPublisherRole
        {
            public SampleAppPublisher(DataPublisherRole publisherRole) : base(publisherRole)
            {
            }

            public override void Start()
            {
                var testData = Encoding.UTF8.GetBytes($"TestString");
                this.Publish(testData);
            }

            public override void Stop()
            {
                
            }
        }
        
        [ApplicationTemplate]
        private class SampleAppReceiver : ApplicationReceiverRole
        {
            public SampleAppReceiver(DataReceiverRole receiverRole) : base(receiverRole)
            {
            }

            public override void Start()
            {
                this.Listen((message) =>
                {
                   
                });
            }

            public override void Stop()
            {
                this.StopListen();
            }
        }
        
        [ApplicationTemplate]
        private class SampleBrokenAppReceiver : ApplicationReceiverRole
        {
            public SampleBrokenAppReceiver(DataReceiverRole receiverRole) : base(receiverRole)
            {
            }

            public override void Start()
            {
                this.Listen((message) =>
                {
                   throw new Exception("Something went wrong");
                });
            }

            public override void Stop()
            {
                this.StopListen();
            }
        }
    }
}