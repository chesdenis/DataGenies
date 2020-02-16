using System;
using DataGenies.Core.Attributes;
using DataGenies.Core.Behaviours;
using DataGenies.Core.Containers;
using DataGenies.Core.Extensions;
using DataGenies.Core.Publishers;
using DataGenies.InMemory;

namespace DataGenies.Core.Tests.Integration.Mocks.Behaviours
{
    [BehaviourTemplate]
    public class MockHandleErroredMessagesBehavior : BehaviourOnException
    {
        public override void Execute(IContainer arg, Exception exception)
        {
            var publisher = arg.Resolve<IPublisher>();
            var originalMessage = arg.Resolve<MqMessage>();
            
            var exceptionMessage = new MqExceptionMessage
            {
                Body = originalMessage.Body,
                Exception = exception
            }.ToBytes();
            
            publisher.Publish(exceptionMessage, "Errors");
        }

        public override BehaviourScope BehaviourScope { get; set; } = BehaviourScope.Message;
        public override BehaviourType BehaviourType { get; set; } = BehaviourType.OnException;
    }
}