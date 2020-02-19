using System;
using System.Collections.Generic;
using DataGenies.Core.Attributes;
using DataGenies.Core.Behaviours;
using DataGenies.Core.Containers;
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
            IEnumerable<BehaviourTemplate> behaviourTemplates, IEnumerable<WrapperBehaviourTemplate> wrapperBehaviours)
            : base(container, publisher, receiver, behaviourTemplates, wrapperBehaviours)
        {
            this.Container.Register<MockReceiverProperties>(new MockReceiverProperties());
        }

        private MockReceiverProperties ReceiverProperties => this.Container.Resolve<MockReceiverProperties>();
         
        protected override void OnStart()
        {
            throw new Exception("Something went wrong");
        }
 
        protected override void OnStop()
        {
            this.StopListen();
        }
    }
}