using System;
using System.Collections.Generic;
using DataGenies.Core.Attributes;
using DataGenies.Core.Behaviours;
using DataGenies.Core.Containers;
using DataGenies.Core.Extensions;
using DataGenies.Core.Publishers;
using DataGenies.Core.Receivers;
using DataGenies.Core.Services;
using DataGenies.Core.Tests.Integration.Mocks.Properties;

namespace DataGenies.Core.Tests.Integration.Mocks.ApplicationTemplates
{
    [ApplicationTemplate]
    public class MockBrokenMessageReceiverAndPublisher : ManagedReceiverAndPublisherServiceWithContainer
    {
        public MockBrokenMessageReceiverAndPublisher(IContainer container, IPublisher publisher, IReceiver receiver,
            IEnumerable<BehaviourTemplate> behaviourTemplates, IEnumerable<WrapperBehaviourTemplate> wrapperBehaviours)
            : base(container, publisher, receiver, behaviourTemplates, wrapperBehaviours)
        {
            this.Container.Register<MockReceiverProperties>(new MockReceiverProperties());
        }

        private MockReceiverProperties ReceiverProperties => this.Container.Resolve<MockReceiverProperties>();
         
        protected override void OnStart()
        {
            this.Listen((message) =>
            {
                ReceiverProperties.ReceivedMessages.Add(message.Body.FromBytes<string>());
                throw new Exception("Something went wrong");
            });
        }
 
        protected override void OnStop()
        {
            this.StopListen();
        }
    }
}