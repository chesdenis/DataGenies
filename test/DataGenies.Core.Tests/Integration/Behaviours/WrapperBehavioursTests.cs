﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataGenies.Core.Behaviours;
using DataGenies.Core.Containers;
using DataGenies.Core.Publishers;
using DataGenies.Core.Tests.Integration.Extensions;
using DataGenies.Core.Tests.Integration.Mocks.ApplicationTemplates;
using DataGenies.Core.Tests.Integration.Mocks.Properties;
using DataGenies.InMemory;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataGenies.Core.Tests.Integration.Converters
{
    [TestClass]
    public class WrapperBehaviourTemplatesTests : BaseIntegrationTest
    {
        [TestInitialize]
        public override void Initialize()
        {
            base.Initialize();
            
            ApplicationTemplatesScanner.RegisterMockApplicationTemplate(typeof(MessagePublisher),"SampleAppPublisherTemplate");
            ApplicationTemplatesScanner.RegisterMockApplicationTemplate(typeof(MockSimpleReceiver), "SampleAppReceiverTemplate");
    
            BehaviourTemplatesScanner.RegisterMockBehaviourTemplate(typeof(ConvertMessagesDuringRunBehaviour), "SampleBehaviourTemplate");
        }
    
        [TestMethod]
        public void ConversionBehaviourShouldChangeMessagesBeforePublish()
        {
             // Arrange
            var publisherId = 1;
            InMemorySchemaDataBuilder.CreateApplicationTemplate(
                    "SampleAppPublisherTemplate",
                    "2019.1.1")
                .CreateApplicationInstance("SampleAppPublisher", publisherId)
                .CreateBehaviourTemplate("SampleBehaviourTemplate", "2019.1.1")
                .CreateBehaviourInstance("SampleBehaviour", BehaviourType.Unspecified, BehaviourScope.Message)
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
    
            Assert.AreEqual(new string("TestString".Reverse().ToArray()), publisherProperties.PublishedMessages[0]);
            Assert.AreEqual(new string("TestString".Reverse().ToArray()), receiverProperties.ReceivedMessages[0]); 
        }
        
        private class MessagePublisher : MockSimplePublisher
        {
            public MessagePublisher(IContainer container, IPublisher publisher,
                IEnumerable<BehaviourTemplate> behaviourTemplates,
                IEnumerable<WrapperBehaviourTemplate> wrapperBehaviours) : base(container, publisher,
                behaviourTemplates, wrapperBehaviours)
            {
            }
    
            protected override void OnStart()
            {
                var testString = $"TestString";
            
                var testData = Encoding.UTF8.GetBytes(testString);
                var mqMessage = new MqMessage()
                {
                    Body = testData
                };
                this.Publish(mqMessage);
    
                Properties.PublishedMessages.Add(Encoding.UTF8.GetString(mqMessage.Body));
            }
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
        
        private class HandleErroredMessagesBehavior : BehaviourTemplate
        {
            // public override void Execute(MqMessage message, Exception exception)
            // {
            //     var exceptionMessage = new MqExceptionMessage
            //     {
            //         Body = originalMessage.Body,
            //         Exception = exception,
            //         RoutingKey = "Errors"
            //     };
            //     
            // }
            //
            // public override void Execute(IContainer arg, Exception exception)
            // {
            //     var publisher = arg.Resolve<IPublisher>();
            //     var originalMessage = arg.Resolve<MqMessage>();
            //
            //     var exceptionMessage = new MqExceptionMessage
            //     {
            //         Body = originalMessage.Body,
            //         Exception = exception,
            //         RoutingKey = "Errors"
            //     };
            //
            //     publisher.Publish(exceptionMessage);
            // }
        }
    }
}