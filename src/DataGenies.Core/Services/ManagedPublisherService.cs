using System;
using System.Collections.Generic;
using DataGenies.Core.Behaviours;
using DataGenies.Core.Extensions;
using DataGenies.Core.Publishers;
using DataGenies.Core.Roles;
using DataGenies.Core.Wrappers;

namespace DataGenies.Core.Services
{
    public abstract class ManagedPublisherService : IPublisher, IManagedService
    {
        private readonly IPublisher _publisher;
        public IEnumerable<IBasicBehaviour> BasicBehaviours { get; }
        public IEnumerable<IBehaviourOnException> BehaviourOnExceptions { get; }
        public IEnumerable<IWrapperBehaviour> WrapperBehaviours { get; }

        protected ManagedPublisherService(
            IPublisher publisher, 
            IEnumerable<IBasicBehaviour> basicBehaviours,
            IEnumerable<IBehaviourOnException> behaviourOnExceptions,
            IEnumerable<IWrapperBehaviour> wrapperBehaviours)
        {
            _publisher = publisher;
            BasicBehaviours = basicBehaviours;
            BehaviourOnExceptions = behaviourOnExceptions;
            WrapperBehaviours = wrapperBehaviours;
        }
         
        public void Publish(byte[] data)
        {
            this.ManagedAction(() => _publisher.Publish(data));
        }

        public void Publish(byte[] data, string routingKey)
        {
            this.ManagedAction(() => _publisher.Publish(data, routingKey));
        }

        public void Publish(byte[] data, IEnumerable<string> routingKeys)
        {
            this.ManagedAction(() =>  _publisher.Publish(data, routingKeys));
        }

        public void PublishRange(IEnumerable<byte[]> dataRange)
        {
            this.ManagedAction(() => _publisher.PublishRange(dataRange));
        }

        public void PublishRange(IEnumerable<byte[]> dataRange, string routingKey)
        {
            this.ManagedAction(() =>  _publisher.PublishRange(dataRange, routingKey));
        }

        public void PublishTuples(IEnumerable<Tuple<byte[], string>> tuples)
        {
            this.ManagedAction(() =>  _publisher.PublishTuples(tuples));
        }

        public abstract void Start();
        public abstract void Stop();
    }
}