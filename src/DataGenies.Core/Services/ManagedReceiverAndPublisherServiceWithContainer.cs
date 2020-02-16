using System;
using System.Collections.Generic;
using DataGenies.Core.Behaviours;
using DataGenies.Core.Containers;
using DataGenies.Core.Extensions;
using DataGenies.Core.Publishers;
using DataGenies.Core.Receivers;
using DataGenies.Core.Wrappers;

namespace DataGenies.Core.Services
{
    public abstract class ManagedReceiverAndPublisherServiceWithContainer : IPublisher, IReceiver, IManagedService,
        IRestartable
    {
        protected readonly IContainer Container;
        
        private readonly IPublisher _publisher;
        private readonly IReceiver _receiver;
        public IEnumerable<IBasicBehaviour> BasicBehaviours { get; }
        public IEnumerable<IBehaviourOnException> BehaviourOnExceptions { get; }
        public IEnumerable<IWrapperBehaviour> WrapperBehaviours { get; }
        
        protected ManagedReceiverAndPublisherServiceWithContainer(
            IContainer container,
            IPublisher publisher, 
            IReceiver receiver, 
            IEnumerable<IBasicBehaviour> basicBehaviours,
            IEnumerable<IBehaviourOnException> behaviourOnExceptions,
            IEnumerable<IWrapperBehaviour> wrapperBehaviours)
        {
            _receiver = receiver;
            _publisher = publisher;
            BasicBehaviours = basicBehaviours;
            BehaviourOnExceptions = behaviourOnExceptions;
            WrapperBehaviours = wrapperBehaviours;
        }
        
        public void Publish(byte[] data)
        {
            this.ManagedAction(container =>_publisher.Publish(data), this.Container);
        }

        public void Publish(byte[] data, string routingKey)
        {
            this.ManagedAction(container => _publisher.Publish(data, routingKey), this.Container);
        }

        public void Publish(byte[] data, IEnumerable<string> routingKeys)
        {
            this.ManagedAction(container => _publisher.Publish(data, routingKeys), this.Container);
        }

        public void PublishRange(IEnumerable<byte[]> dataRange)
        {
            this.ManagedAction(container => _publisher.PublishRange(dataRange), this.Container);
        }

        public void PublishRange(IEnumerable<byte[]> dataRange, string routingKey)
        {
            this.ManagedAction(container =>  _publisher.PublishRange(dataRange, routingKey), this.Container);
        }

        public void PublishTuples(IEnumerable<Tuple<byte[], string>> tuples)
        {
            this.ManagedAction(container => _publisher.PublishTuples(tuples), this.Container);
        }

        public void Listen(Action<byte[]> onReceive)
        {
            _receiver.Listen(arg =>
                this.ManagedAction(container => { onReceive(arg); }, Container));
        }

        public void StopListen()
        {
            this.ManagedAction(container => _receiver.StopListen(), Container);
        }

        public abstract void Start();
        public abstract void Stop();
    }
}