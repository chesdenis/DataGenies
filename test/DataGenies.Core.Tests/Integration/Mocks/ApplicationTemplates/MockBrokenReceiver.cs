using System;
using System.Collections.Generic;
using System.Text;
using DataGenies.Core.Attributes;
using DataGenies.Core.Behaviours;
using DataGenies.Core.Containers;
using DataGenies.Core.Receivers;
using DataGenies.Core.Services;
using DataGenies.Core.Tests.Integration.Mocks.Properties;
using DataGenies.Core.Wrappers;

namespace DataGenies.Core.Tests.Integration.Mocks.ApplicationTemplates
{
    [ApplicationTemplate]
    public class MockBrokenReceiver : ManagedReceiverServiceWithContainer
    {
        public MockBrokenReceiver(IContainer container, IReceiver receiver,
            IEnumerable<BehaviourTemplate> behaviourTemplates, IEnumerable<WrapperBehaviourTemplate> wrapperBehaviours)
            : base(container, receiver, behaviourTemplates, wrapperBehaviours)
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