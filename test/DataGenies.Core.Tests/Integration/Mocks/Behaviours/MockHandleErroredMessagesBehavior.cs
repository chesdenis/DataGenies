using System;
using DataGenies.Core.Attributes;
using DataGenies.Core.Behaviours;
using DataGenies.Core.Extensions;
using DataGenies.Core.Publishers;
using DataGenies.Core.Tests.Integration.Mocks.Properties;
using DataGenies.InMemory;

namespace DataGenies.Core.Tests.Integration.Mocks.Behaviours
{
    [BehaviourTemplate]
    public class MockHandleErroredMessagesBehavior : GenericBehaviour<MockBehaviourProperties>
    {
        public override BehaviourType Type { get; set; } = BehaviourType.OnException;
       
        public override void DoSomethingOnException(byte[] message, Exception ex = null)
        {
            var publisher = this.ContextContainer.Resolve<IPublisher>();
           
            var exceptionMessage = new MqExceptionMessage
            {
                Body = message,
                Exception = ex
            }.ToBytes();

            publisher.Publish(exceptionMessage, "Errors");
        }
    }
}