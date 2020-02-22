using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataGenies.Core.Behaviours;
using DataGenies.Core.Configurators;
using DataGenies.Core.Containers;
using DataGenies.Core.Extensions;
using DataGenies.Core.Models;
using DataGenies.Core.Publishers;
using DataGenies.Core.Receivers;
using DataGenies.Core.Services;
using DataGenies.Core.Tests.Integration.Extensions;
using DataGenies.Core.Tests.Integration.Mocks.ApplicationTemplates;
using DataGenies.Core.Tests.Integration.Mocks.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataGenies.Core.Tests.Integration.Behaviours
{
    [TestClass]
    public class ServiceScopeBehaviourTemplatesTests : BaseIntegrationTest
    {
        [TestInitialize]
        public override void Initialize()
        {
            base.Initialize();
            
            ApplicationTemplatesScanner.RegisterMockApplicationTemplate(typeof(MessageWithPrefixPublisher),
                "SampleAppPublisherTemplate");
            ApplicationTemplatesScanner.RegisterMockApplicationTemplate(typeof(MockSimpleReceiver),
                "SampleAppReceiverTemplate");
            ApplicationTemplatesScanner.RegisterMockApplicationTemplate(typeof(MockBrokenReceiver),
                "SampleBrokenReceiverTemplate");
            
            BehaviourTemplatesScanner.RegisterMockBehaviourTemplate(typeof(ChangeParameterInContainerBehaviour), 
                "ChangeParameterInContainerBehaviourTemplate");  
            BehaviourTemplatesScanner.RegisterMockBehaviourTemplate(typeof(OnExceptionChangeParameterInContainerBehaviour), 
                "OnExceptionChangeParameterInContainerBehaviourTemplate");
            BehaviourTemplatesScanner.RegisterMockBehaviourTemplate(typeof(SomePublisherMessagesDuringStartBehaviour),
                "SomePublisherMessagesDuringStartBehaviourTemplate");
        }
        
        [TestMethod]
        public void BeforeRunBehaviourShouldAffectFlow()
        {
            // Arrange
            var publisherId = 1;
            InMemorySchemaDataBuilder.CreateApplicationTemplate(
                    "SampleAppPublisherTemplate",
                    "2019.1.1")
                .CreateApplicationInstance("SampleAppPublisher", publisherId)
                .CreateBehaviourTemplate("ChangeParameterInContainerBehaviourTemplate", "2019.1.1")
                .CreateBehaviourInstance("ChangeParameterInContainerBehaviour", BehaviourType.BeforeRun, BehaviourScope.Service)
                .AssignBehaviour();

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
            var publisherProperties = Orchestrator.GetApplicationInstanceContainer(publisherId).Resolve<MockPublisherProperties>();
            var receiverProperties = Orchestrator.GetApplicationInstanceContainer(receiverId).Resolve<MockReceiverProperties>();
            
            Assert.AreEqual("PrefixTestString", publisherProperties.PublishedMessages[0]);
            Assert.AreEqual("PrefixTestString", receiverProperties.ReceivedMessages[0]);
        }
        
        [TestMethod]
        public void AfterRunBehaviourShouldAffectFlow()
        {
            // Arrange
            var publisherId = 1;
            InMemorySchemaDataBuilder.CreateApplicationTemplate(
                    "SampleAppPublisherTemplate",
                    "2019.1.1")
                .CreateApplicationInstance("SampleAppPublisher", publisherId)
                .CreateBehaviourTemplate("ChangeParameterInContainerBehaviourTemplate", "2019.1.1")
                .CreateBehaviourInstance("ChangeParameterInContainerBehaviour", BehaviourType.AfterRun, BehaviourScope.Service)
                .AssignBehaviour();

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
                await Orchestrator.RestartAll();
                await Task.Delay(1000);
            }).Wait();

            // Assert
            var publisherProperties = Orchestrator.GetApplicationInstanceContainer(publisherId).Resolve<MockPublisherProperties>();
            var receiverProperties = Orchestrator.GetApplicationInstanceContainer(receiverId).Resolve<MockReceiverProperties>();

            Assert.AreEqual(2, publisherProperties.PublishedMessages.Count());
            Assert.AreEqual(2, receiverProperties.ReceivedMessages.Count());
            
            Assert.AreEqual("TestString", publisherProperties.PublishedMessages[0]);
            Assert.AreEqual("TestString", receiverProperties.ReceivedMessages[0]);
            
            Assert.AreEqual("PrefixTestString", publisherProperties.PublishedMessages[1]);
            Assert.AreEqual("PrefixTestString", receiverProperties.ReceivedMessages[1]);
        }

        [TestMethod]
        public void OnExceptionBehaviourShouldAffectFlow()
        {
            // Arrange
            var publisherId = 1;
            InMemorySchemaDataBuilder.CreateApplicationTemplate(
                    "SampleAppPublisherTemplate",
                    "2019.1.1")
                .CreateApplicationInstance("SampleAppPublisher", publisherId);

            var receiverId = 2;
            InMemorySchemaDataBuilder.CreateApplicationTemplate(
                    "SampleBrokenReceiverTemplate",
                    "2018.1.1")
                .CreateApplicationInstance("SampleBrokenReceiver", receiverId)
                .CreateBehaviourTemplate("OnExceptionChangeParameterInContainerBehaviourTemplate", "2019.1.1")
                .CreateBehaviourInstance("OnExceptionChangeParameterInContainerBehaviour", BehaviourType.OnException, BehaviourScope.Service)
                .AssignBehaviour();;
            
            InMemorySchemaDataBuilder.ConfigureBinding(
                "SampleAppPublisher", 
                "SampleBrokenReceiver", "#");
            
            Orchestrator.Deploy(publisherId);
            Orchestrator.Deploy(receiverId);
        
            // Act
            Orchestrator.Start(publisherId);
            Orchestrator.Start(receiverId);
            
            Task.Run(async () =>
            {
                await Task.Delay(1000);
                await Orchestrator.Stop(receiverId);
                await Orchestrator.RestartAll();
                await Task.Delay(1000);
            }).Wait();

            // Assert
            var publisherProperties = Orchestrator.GetApplicationInstanceContainer(publisherId).Resolve<MockPublisherProperties>();
            var receiverProperties = Orchestrator.GetApplicationInstanceContainer(receiverId).Resolve<MockReceiverProperties>();
            
            Assert.AreEqual("TestString", publisherProperties.PublishedMessages[0]);
            Assert.AreEqual("TestString", receiverProperties.ReceivedMessages[0]);
        }

        [TestMethod]
        public void WrapperBehaviourShouldAffectFlow()
        {
            // Arrange
            var publisherId = 1;
            InMemorySchemaDataBuilder.CreateApplicationTemplate(
                    "SampleAppPublisherTemplate",
                    "2019.1.1")
                .CreateApplicationInstance("SampleAppPublisher", publisherId)
                .CreateBehaviourTemplate("SomePublisherMessagesDuringStartBehaviourTemplate", "2019.1.1")
                .CreateBehaviourInstance("SomePublisherMessagesDuringStartBehaviour", BehaviourType.Unspecified, BehaviourScope.Service)
                .AssignBehaviour();

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
            var publisherProperties = Orchestrator.GetApplicationInstanceContainer(publisherId).Resolve<MockPublisherProperties>();
            var receiverProperties = Orchestrator.GetApplicationInstanceContainer(receiverId).Resolve<MockReceiverProperties>();
            
            Assert.AreEqual("TestString", publisherProperties.PublishedMessages[0]);
            
            Assert.AreEqual("\"Started\"", receiverProperties.ReceivedMessages[0]);
            Assert.AreEqual("TestString", receiverProperties.ReceivedMessages[1]);
        }
        
        private class MessageWithPrefixPublisher : MockSimplePublisher
        {
            public MessageWithPrefixPublisher(IContainer container, IPublisher publisher, IReceiver receiver, IEnumerable<BehaviourTemplate> behaviourTemplates, IEnumerable<WrapperBehaviourTemplate> wrapperBehaviours, ISchemaDataContext schemaDataContext, IBindingConfigurator bindingConfigurator) : base(container, publisher, receiver, behaviourTemplates, wrapperBehaviours, schemaDataContext, bindingConfigurator)
            {
            }

            protected override void OnStart()
            {
                var testString = $"{this.Properties.ManagedParameter}TestString";
            
                var testData = Encoding.UTF8.GetBytes(testString);
                this.Publish(new MqMessage()
                {
                    Body = testData
                });
            
                Properties.PublishedMessages.Add(testString);
            }
        }
        
        private class ChangeParameterInContainerBehaviour : BehaviourTemplate
        {
            public override void Execute(IContainer arg)
            {
                arg.Resolve<MockPublisherProperties>().ManagedParameter = "Prefix";
            }
        }
        
        private class OnExceptionChangeParameterInContainerBehaviour : BehaviourTemplate
        {
            public override void Execute(IContainer container, Exception exception)
            {
                container.Resolve<MockReceiverProperties>().ManagedParameter = "Work";
            }
        }
        
        private class SomePublisherMessagesDuringStartBehaviour : WrapperBehaviourTemplate
        {
            public override BehaviourScope BehaviourScope { get; set; } = BehaviourScope.Service;

            public override void BehaviourActionWithContainer<T>(Action<T> action, T container)
            {
                ((IPublisher)this.ManagedService).Publish(new MqMessage
                {
                    Body = "Started".ToBytes()
                });
                
                action(container);
            }
        }
        
        private class MockBrokenReceiver : ManagedService
        {
            public MockBrokenReceiver(IContainer container, IPublisher publisher, IReceiver receiver, IEnumerable<BehaviourTemplate> behaviourTemplates, IEnumerable<WrapperBehaviourTemplate> wrapperBehaviours, ISchemaDataContext schemaDataContext, IBindingConfigurator bindingConfigurator) : base(container, publisher, receiver, behaviourTemplates, wrapperBehaviours, schemaDataContext, bindingConfigurator)
            {
                this.Container.Register<MockReceiverProperties>(new MockReceiverProperties());
            }

            private MockReceiverProperties Properties => this.Container.Resolve<MockReceiverProperties>();
         
            protected override void OnStart()
            {
                if (Properties.ManagedParameter == "Work")
                {
                    this.Listen((message) =>
                    {
                        Properties.ReceivedMessages.Add(
                            Encoding.UTF8.GetString(message.Body));
                    });
                }

                throw new Exception("Something went wrong");
            }
 
            protected override void OnStop()
            {
                this.StopListen();
            }
        }
    }
}