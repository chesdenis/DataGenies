using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataGenies.Core.Behaviours;
using DataGenies.Core.Containers;
using DataGenies.Core.Publishers;
using DataGenies.Core.Receivers;
using DataGenies.Core.Tests.Integration.Extensions;
using DataGenies.Core.Tests.Integration.Mocks.ApplicationTemplates;
using DataGenies.Core.Tests.Integration.Mocks.Properties;
using DataGenies.InMemory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute.Exceptions;

namespace DataGenies.Core.Tests.Integration.Behaviours
{
    [TestClass]
    public class MessageScopeBehaviourTemplateTests : BaseIntegrationTest
    {
        [TestInitialize]
        public override void Initialize()
        {
            base.Initialize();
            
            ApplicationTemplatesScanner.RegisterMockApplicationTemplate(typeof(Publish5MessagesPublisher),
                "Publish5MessagesPublisherTemplate");
            ApplicationTemplatesScanner.RegisterMockApplicationTemplate(typeof(ReceiveFirst2MessagesAndThrowErrorReceiver),
                "ReceiveFirst2MessagesAndThrowErrorReceiverTemplate");
            
            ApplicationTemplatesScanner.RegisterMockApplicationTemplate(typeof(MockSimpleReceiver),
                "SimpleReceiverTemplate");
            
            BehaviourTemplatesScanner.RegisterMockBehaviourTemplate(typeof(ConvertMessagesDuringRunBehaviour), 
                "ConvertMessagesDuringRunBehaviourTemplate");
            BehaviourTemplatesScanner.RegisterMockBehaviourTemplate(typeof(SendErroredMessagesBehavior), 
                "SendErroredMessagesBehaviorTemplate");
        }
        
        [TestMethod]
        public void WrappedBehaviourShouldAffectFlow()
        {
            // Arrange
            var publisherId = 1;
            InMemorySchemaDataBuilder.CreateApplicationTemplate(
                    "Publish5MessagesPublisherTemplate",
                    "2019.1.1")
                .CreateApplicationInstance("SampleAppPublisher", publisherId)
                .CreateBehaviourTemplate("ConvertMessagesDuringRunBehaviourTemplate", "2019.1.1")
                .CreateBehaviourInstance("ConvertMessagesDuringRunBehaviour", BehaviourType.Unspecified, BehaviourScope.Message)
                .AssignBehaviour();
            
            var receiverId = 2;
            InMemorySchemaDataBuilder.CreateApplicationTemplate(
                    "SimpleReceiverTemplate",
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
    
            Assert.AreEqual(new string("TestString0".Reverse().ToArray()), publisherProperties.PublishedMessages[0]);
            Assert.AreEqual(new string("TestString0".Reverse().ToArray()), receiverProperties.ReceivedMessages[0]); 
        }

         [TestMethod]
        public void OnExceptionBehaviourShouldAffectFlow()
        {
            // Arrange
            var publisherId = 1;
            InMemorySchemaDataBuilder.CreateApplicationTemplate(
                    "Publish5MessagesPublisherTemplate",
                    "2019.1.1")
                .CreateApplicationInstance("SampleAppPublisher", publisherId);

            var brokenAppReceiverId = 2;
            InMemorySchemaDataBuilder.CreateApplicationTemplate(
                    "ReceiveFirst2MessagesAndThrowErrorReceiverTemplate",
                    "2018.1.1")
                .CreateApplicationInstance("BrokenAppReceiver", brokenAppReceiverId);

            var receiverId = 3;
            InMemorySchemaDataBuilder.CreateApplicationTemplate(
                    "SimpleReceiverTemplate",
                    "2018.1.1")
                .CreateApplicationInstance("SampleAppReceiver", receiverId);

            InMemorySchemaDataBuilder
                .UsingExistingApplicationInstance("SampleAppPublisher")
                .CreateBehaviourTemplate("ConvertMessagesDuringRunBehaviourTemplate", "2019.1.1")
                .CreateBehaviourInstance("ConvertMessagesDuringRunBehaviour", BehaviourType.Unspecified,
                    BehaviourScope.Message)
                .AssignBehaviour();

            InMemorySchemaDataBuilder
                .UsingExistingApplicationInstance("SampleAppReceiver")
                .CreateBehaviourTemplate("SendErroredMessagesBehaviorTemplate", "2019.1.1")
                .CreateBehaviourInstance("SendErroredMessagesBehavior", BehaviourType.Unspecified,
                    BehaviourScope.Message)
                .AssignBehaviour();
             
            InMemorySchemaDataBuilder.ConfigureBinding(
                "SampleAppPublisher",
                "BrokenAppReceiver", "#");
            
            InMemorySchemaDataBuilder.ConfigureBinding(
                "BrokenAppReceiver",
                "SampleAppReceiver", "#");
          
            Orchestrator.Deploy(publisherId);
            Orchestrator.Deploy(brokenAppReceiverId);
            Orchestrator.Deploy(receiverId);
            
            // Act
            Orchestrator.Start(publisherId);
            Orchestrator.Start(brokenAppReceiverId);
            Orchestrator.Start(receiverId);
            
            Task.Run(async () =>
            {
                await Task.Delay(1000);
                await Orchestrator.Stop(receiverId);
                await Orchestrator.Stop(brokenAppReceiverId);
            }).Wait();
    
            // Assert
            // var publisherProperties = Orchestrator.GetApplicationInstanceContainer(publisherId).Resolve<MockPublisherProperties>();
            // var receiverProperties = Orchestrator.GetApplicationInstanceContainer(receiverId).Resolve<MockReceiverProperties>();
            //
            // Assert.AreEqual(new string("TestString0".Reverse().ToArray()), publisherProperties.PublishedMessages[0]);
            // Assert.AreEqual(new string("TestString0".Reverse().ToArray()), receiverProperties.ReceivedMessages[0]); 
        }
        
        private class ConvertMessagesDuringRunBehaviour : WrapperBehaviourTemplate
        {
            public override BehaviourScope BehaviourScope { get; set; } = BehaviourScope.Message;
    
            public override void BehaviourActionWithMessage<T>(Action<T> action, T message)
            {
                var originalString = Encoding.UTF8.GetString(message.Body);
                var reversedString = new string(originalString.Reverse().ToArray());
                
                message.Body = Encoding.UTF8.GetBytes(reversedString);
    
                action(message);
            }
        }
        
        private class SendErroredMessagesBehavior : BehaviourTemplate
        {
            public override void Execute(MqMessage message, Exception exception)
            {
                var exceptionMessage = new MqExceptionMessage
                {
                    Body = message.Body,
                    Exception = exception,
                    RoutingKey = "Errors"
                };
                
                ((IPublisher)this.ManagedService).Publish(exceptionMessage);
            }
        }
        
        private class Publish5MessagesPublisher : MockSimplePublisher
        {
            public Publish5MessagesPublisher(IContainer container, IPublisher publisher,
                IEnumerable<BehaviourTemplate> behaviourTemplates,
                IEnumerable<WrapperBehaviourTemplate> wrapperBehaviours) : base(container, publisher,
                behaviourTemplates, wrapperBehaviours)
            {
            }
    
            protected override void OnStart()
            {
                for (int i = 0; i < 5; i++)
                {
                    var testString = $"TestString{i}";
            
                    var testData = Encoding.UTF8.GetBytes(testString);
                    var mqMessage = new MqMessage()
                    {
                        Body = testData
                    };
                    this.Publish(mqMessage);
    
                    Properties.PublishedMessages.Add(Encoding.UTF8.GetString(mqMessage.Body));
                }
               
            }
        }
        
        private class ReceiveFirst2MessagesAndThrowErrorReceiver : MockSimpleReceiver
        {
            public ReceiveFirst2MessagesAndThrowErrorReceiver(IContainer container, IReceiver receiver,
                IEnumerable<BehaviourTemplate> behaviourTemplates,
                IEnumerable<WrapperBehaviourTemplate> wrapperBehaviours) : base(container, receiver, behaviourTemplates,
                wrapperBehaviours)
            {
            }
            
            private MockReceiverProperties Properties => this.Container.Resolve<MockReceiverProperties>();

            protected override void OnStart()
            {
                int receivedMessages = 0;
                
                this.Listen((message) =>
                {
                    if (receivedMessages > 1)
                    {
                        throw new Exception();
                    }

                    Properties.ReceivedMessages.Add(
                        Encoding.UTF8.GetString(message.Body));

                    receivedMessages++;
                });
            }
        }
    }
}