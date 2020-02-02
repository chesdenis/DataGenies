using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataGenies.Core.Attributes;
using DataGenies.Core.Roles;
using DataGenies.Core.Tests.Integration.Mocks;
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
        public void PublisherRoleShouldDeliverMessagesToReceiver()
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
                .UsingTemplateType(typeof(MockAppTemplateSimplePublisher)).Build();
            
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
                    receiverManagedApp.Stop();
                });
                
                receiverManagedApp.Start();
            });
            
            Task.WaitAll(t1, t2);
            
            // Assert
            Assert.AreEqual("TestString", ((MockBasicAppTemplatePublisher) (publisherManagedApp.GetRootComponent())).GetLastMessageAsString());
            Assert.AreEqual("TestString", ((MockBasicAppTemplateReceiver) (receiverManagedApp.GetRootComponent())).GetLastMessageAsString());
        }
        
        [TestMethod]
        public void PublisherRoleShouldDeliverOneMessageToMultipleReceivers()
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
            
            var publisherManagedApp = _managedApplicationBuilder.UsingApplicationInstance(
                    _inMemorySchemaContext.ApplicationInstances
                        .First(f => f.Name == "SampleAppPublisher"))
                .UsingTemplateType(typeof(MockAppTemplateSimplePublisher)).Build();
            
            var receiverManagedAppA = _managedApplicationBuilder.UsingApplicationInstance(
                    _inMemorySchemaContext.ApplicationInstances
                        .First(f => f.Name == "SampleAppReceiverA"))
                .UsingTemplateType(typeof(MockAppTemplateSimpleReceiver)).Build();
            
            var receiverManagedAppB = _managedApplicationBuilder.UsingApplicationInstance(
                    _inMemorySchemaContext.ApplicationInstances
                        .First(f => f.Name == "SampleAppReceiverB"))
                .UsingTemplateType(typeof(MockAppTemplateSimpleReceiver)).Build();
            
            var t1 = Task.Run(() => publisherManagedApp.Start());
            
            var t2 = Task.Run(() =>
            {
                Task.Run(async () =>
                {
                    await Task.Delay(1000);
                    receiverManagedAppA.Stop();
                });
                
                receiverManagedAppA.Start();
            });
            
            var t3 = Task.Run(() =>
            {
                Task.Run(async () =>
                {
                    await Task.Delay(1000);
                    receiverManagedAppB.Stop();
                });
                
                receiverManagedAppB.Start();
            });

            Task.WaitAll(t1, t2, t3);
            // Assert
            Assert.AreEqual(1, ((MockBasicAppTemplateReceiver) (receiverManagedAppA.GetRootComponent())).GetMessagesCountInState());
            Assert.AreEqual(1,((MockBasicAppTemplateReceiver) (receiverManagedAppB.GetRootComponent())).GetMessagesCountInState());
            
            Assert.AreEqual("TestString", ((MockBasicAppTemplatePublisher) (publisherManagedApp.GetRootComponent())).GetLastMessageAsString());
            Assert.AreEqual("TestString", ((MockBasicAppTemplateReceiver) (receiverManagedAppA.GetRootComponent())).GetLastMessageAsString());
            Assert.AreEqual("TestString", ((MockBasicAppTemplateReceiver) (receiverManagedAppB.GetRootComponent())).GetLastMessageAsString());
        }
        
        [TestMethod]
        public void PublisherRoleShouldDeliverMultipleMessagesToMultipleReceivers()
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
                "SampleAppReceiverA", "2");
            
            _schemaDataBuilder.ConfigureBinding(
                "SampleAppPublisher", 
                "SampleAppReceiverB", "3");
            
            var publisherManagedApp = _managedApplicationBuilder.UsingApplicationInstance(
                    _inMemorySchemaContext.ApplicationInstances
                        .First(f => f.Name == "SampleAppPublisher"))
                .UsingTemplateType(typeof(MockAppTemplatePublisherWhichPushMultipleMessagesWithDifferentRoutingKeys)).Build();
            
            var receiverManagedAppA = _managedApplicationBuilder.UsingApplicationInstance(
                    _inMemorySchemaContext.ApplicationInstances
                        .First(f => f.Name == "SampleAppReceiverA"))
                .UsingTemplateType(typeof(MockAppTemplateSimpleReceiver)).Build();
            
            var receiverManagedAppB = _managedApplicationBuilder.UsingApplicationInstance(
                    _inMemorySchemaContext.ApplicationInstances
                        .First(f => f.Name == "SampleAppReceiverB"))
                .UsingTemplateType(typeof(MockAppTemplateSimpleReceiver)).Build();
            
            var t1 = Task.Run(() => publisherManagedApp.Start());
            
            var t2 = Task.Run(() =>
            {
                Task.Run(async () =>
                {
                    await Task.Delay(1000);
                    receiverManagedAppA.Stop();
                });
                
                receiverManagedAppA.Start();
            });
            
            var t3 = Task.Run(() =>
            {
                Task.Run(async () =>
                {
                    await Task.Delay(1000);
                    receiverManagedAppB.Stop();
                });
                
                receiverManagedAppB.Start();
            });

            Task.WaitAll(t1, t2, t3);
            // Assert
            Assert.AreEqual(10, ((MockBasicAppTemplatePublisher) (publisherManagedApp.GetRootComponent())).GetMessagesCountInState());
            
            Assert.AreEqual(1, ((MockAppTemplateSimpleReceiver) (receiverManagedAppA.GetRootComponent())).GetMessagesCountInState());
            Assert.AreEqual(1,((MockAppTemplateSimpleReceiver) (receiverManagedAppB.GetRootComponent())).GetMessagesCountInState());
            
            Assert.AreEqual("TestString-2", ((MockAppTemplateSimpleReceiver) (receiverManagedAppA.GetRootComponent())).GetLastMessageAsString());
            Assert.AreEqual("TestString-3", ((MockAppTemplateSimpleReceiver) (receiverManagedAppB.GetRootComponent())).GetLastMessageAsString());
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
                .UsingTemplateType(typeof(MockAppTemplateSimplePublisher)).Build();
            
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
                    receiverManagedApp.Stop();
                });
            
                receiverManagedApp.Start();
            });
            
            Task.WaitAll(t1, t2);
             
            // Assert
            Assert.AreEqual("TestString", ((MockBasicAppTemplatePublisher) (publisherManagedApp.GetRootComponent())).GetLastMessageAsString());
            Assert.AreEqual(0, ((MockBasicAppTemplateReceiver) (receiverManagedApp.GetRootComponent())).GetMessagesCountInState());
        }
    }
}