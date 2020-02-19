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
    public class MockHandleErroredMessagesBehavior : BehaviourTemplate
    {
        public override void Execute(IContainer arg, Exception exception)
        {
            var publisher = arg.Resolve<IPublisher>();
            var originalMessage = arg.Resolve<MqMessage>();
            
            var exceptionMessage = new MqExceptionMessage
            {
                Body = originalMessage.Body,
                Exception = exception,
                RoutingKey = "Errors"
            };
            
            publisher.Publish(exceptionMessage);
        }
    }
}