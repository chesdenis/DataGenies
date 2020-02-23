using System.Collections.Generic;
using System.Text;
using DataGenies.Core.Attributes;
using DataGenies.Core.Behaviours;
using DataGenies.Core.Configurators;
using DataGenies.Core.Containers;
using DataGenies.Core.Extensions;
using DataGenies.Core.Models;
using DataGenies.Core.Publishers;
using DataGenies.Core.Receivers;
using DataGenies.Core.Services;
using DataGenies.Core.Tests.Integration.Mocks.Properties;

namespace DataGenies.Core.Tests.Integration.Mocks.ApplicationTemplates
{
    [ApplicationTemplate]
    public class MockSimpleReceiver : ManagedService
    {
        public MockSimpleReceiver(IContainer container, IPublisher publisher, IReceiver receiver,
            IEnumerable<BehaviourTemplate> behaviourTemplates, IEnumerable<WrapperBehaviourTemplate> wrapperBehaviours,
            BindingNetwork bindingNetwork) : base(container, publisher, receiver, behaviourTemplates, wrapperBehaviours,
            bindingNetwork)
        {
            this.Container.Register<MockReceiverProperties>(new MockReceiverProperties());
        }

        private MockReceiverProperties Properties => this.Container.Resolve<MockReceiverProperties>();
        
        protected override void OnStart()
        {
            this.ConnectedPublishers().ListenUsing(this, (message) =>
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