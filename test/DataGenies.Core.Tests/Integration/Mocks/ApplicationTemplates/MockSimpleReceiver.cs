﻿using System.Collections.Generic;
using System.Text;
using DataGenies.Core.Attributes;
using DataGenies.Core.Behaviours;
using DataGenies.Core.Containers;
using DataGenies.Core.Publishers;
using DataGenies.Core.Receivers;
using DataGenies.Core.Services;
using DataGenies.Core.Tests.Integration.Mocks.Properties;

namespace DataGenies.Core.Tests.Integration.Mocks.ApplicationTemplates
{
    [ApplicationTemplate]
    public class MockSimpleReceiver : ManagedCommunicableServiceWithContainer
    {
        public MockSimpleReceiver(IContainer container, IPublisher publisher, IReceiver receiver,
            IEnumerable<BehaviourTemplate> behaviourTemplates, IEnumerable<WrapperBehaviourTemplate> wrapperBehaviours)
            : base(container, publisher, receiver, behaviourTemplates, wrapperBehaviours)
        {
            this.Container.Register<MockReceiverProperties>(new MockReceiverProperties());
        }

        private MockReceiverProperties Properties => this.Container.Resolve<MockReceiverProperties>();
        
        protected override void OnStart()
        {
            this.Listen((message) =>
            {
                Properties.ReceivedMessages.Add(
                    Encoding.UTF8.GetString(message.Body));
            });
        }
 
        protected override void OnStop()
        {
            this.StopListen();
        }
    }
}