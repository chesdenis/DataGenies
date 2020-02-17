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
using DataGenies.Core.Wrappers;

namespace DataGenies.Core.Tests.Integration.Mocks.ApplicationTemplates
{
    [ApplicationTemplate]
    public class MockBrokenReceiverAndPublisher : ManagedReceiverAndPublisherServiceWithContainer
    {
        public MockBrokenReceiverAndPublisher(IContainer container, IPublisher publisher, IReceiver receiver,
            IEnumerable<IBasicBehaviour> basicBehaviours, IEnumerable<IBehaviourOnException> behaviourOnExceptions,
            IEnumerable<IWrapperBehaviour> wrapperBehaviours) : base(container, publisher, receiver, basicBehaviours,
            behaviourOnExceptions, wrapperBehaviours)
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