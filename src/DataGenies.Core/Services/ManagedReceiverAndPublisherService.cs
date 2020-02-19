using System;
using System.Collections.Generic;
using DataGenies.Core.Behaviours;
using DataGenies.Core.Extensions;
using DataGenies.Core.Publishers;
using DataGenies.Core.Receivers;
using DataGenies.Core.Wrappers;
using DataGenies.InMemory;

namespace DataGenies.Core.Services
{
    public abstract class ManagedReceiverAndPublisherService : ManagedService, IPublisher, IReceiver 
    {
        private readonly IPublisher _publisher;
        private readonly IReceiver _receiver;
       
        protected ManagedReceiverAndPublisherService(
            IPublisher publisher, 
            IReceiver receiver, 
            IEnumerable<BehaviourTemplate> behaviourTemplates,
            IEnumerable<WrapperBehaviourTemplate> wrapperBehaviours)
            : base(behaviourTemplates, wrapperBehaviours)
        {
            _receiver = receiver;
            _publisher = publisher;
        }
         
        public void Listen(Action<MqMessage> onReceive)
        {
            this.ManagedAction(() =>
            {
                _receiver.Listen(arg =>
                    this.ManagedActionWithMessage(onReceive, arg, BehaviourScope.Message));
            }, BehaviourScope.Service);
        }

        public void StopListen()
        {
            this.ManagedAction(() => _receiver.StopListen(), BehaviourScope.Service);
        }

        public void Publish(MqMessage data)
        {
            this.ManagedActionWithMessage((x) => _publisher.Publish(x), data, BehaviourScope.Message);
        }

        public void PublishRange(IEnumerable<MqMessage> dataRange)
        {
            this.ManagedAction(() =>
            {
                foreach (var dataEntry in dataRange)
                {
                    this.Publish(dataEntry);
                }
            }, BehaviourScope.Service);
        }
    }
}