using System;
using System.Collections.Generic;
using DataGenies.Core.Behaviours;
using DataGenies.Core.Containers;
using DataGenies.Core.Extensions;
using DataGenies.Core.Publishers;
using DataGenies.Core.Wrappers;

namespace DataGenies.Core.Services
{
    public abstract class ManagedPublisherServiceWithContainer : IPublisher, IRestartable, IManagedService
    {
        protected readonly IContainer Container;
        
        private readonly IPublisher _publisher;
        public IEnumerable<IBasicBehaviour> BasicBehaviours { get; }
        public IEnumerable<IBehaviourOnException> BehaviourOnExceptions { get; }
        public IEnumerable<IWrapperBehaviour> WrapperBehaviours { get; }

        protected ManagedPublisherServiceWithContainer(
            IContainer container,
            IPublisher publisher, 
            IEnumerable<IBasicBehaviour> basicBehaviours,
            IEnumerable<IBehaviourOnException> behaviourOnExceptions,
            IEnumerable<IWrapperBehaviour> wrapperBehaviours)
        {
            Container = container;
            
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

        public abstract void Start();
        public abstract void Stop();
    }
}