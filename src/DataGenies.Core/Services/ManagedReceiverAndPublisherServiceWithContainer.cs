using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using DataGenies.Core.Behaviours;
using DataGenies.Core.Containers;
using DataGenies.Core.Extensions;
using DataGenies.Core.Publishers;
using DataGenies.Core.Receivers;
using DataGenies.Core.Wrappers;
using DataGenies.InMemory;

namespace DataGenies.Core.Services
{
    public abstract class ManagedReceiverAndPublisherServiceWithContainer : ManagedServiceWithContainer, IPublisher, IReceiver
    {
        private readonly IPublisher _publisher;
        private readonly IReceiver _receiver;
      
        protected ManagedReceiverAndPublisherServiceWithContainer(
            IContainer container,
            IPublisher publisher, 
            IReceiver receiver, 
            IEnumerable<BehaviourTemplate> behaviourTemplates,
            IEnumerable<WrapperBehaviourTemplate> wrapperBehaviours)
            : base(container, behaviourTemplates, wrapperBehaviours)
        {
            _receiver = receiver;
            _publisher = publisher;
        }
 
        public void Publish(MqMessage data)
        {
            this.ManagedActionWithMessage((x) => _publisher.Publish(x), data, BehaviourScope.Message);
        }

        public void PublishRange(IEnumerable<MqMessage> dataRange)
        {
            this.ManagedActionWithContainer((x) =>
            {
                foreach (var dataEntry in dataRange)
                {
                    this.Publish(dataEntry);
                }
            }, Container, BehaviourScope.Service);
        }

        public void Listen(Action<MqMessage> onReceive)
        {
            this.ManagedActionWithContainer((x) =>
            {
                _receiver.Listen(arg =>
                    this.ManagedActionWithMessage(onReceive, arg, BehaviourScope.Message));
            }, Container, BehaviourScope.Service);
        }

        public void StopListen()
        {
            this.ManagedActionWithContainer((x) => _receiver.StopListen(), Container ,BehaviourScope.Service);
        }
    }
}