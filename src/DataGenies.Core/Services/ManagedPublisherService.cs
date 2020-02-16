using System;
using System.Collections.Generic;
using DataGenies.Core.Behaviours;
using DataGenies.Core.Extensions;
using DataGenies.Core.Publishers;
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
            this.ManagedAction((x) => _publisher.Publish(x), BehaviourScope.Message, data);
        }

        public void Publish(byte[] data, string routingKey)
        {
            this.ManagedAction((x) => _publisher.Publish(x, routingKey), BehaviourScope.Message, data);
        }

        public void Publish(byte[] data, IEnumerable<string> routingKeys)
        {
            this.ManagedAction((x) =>  _publisher.Publish(x, routingKeys), BehaviourScope.Message, data);
        }

        public void PublishRange(IEnumerable<byte[]> dataRange)
        {
            this.ManagedAction((x) => _publisher.PublishRange(x), BehaviourScope.Message, dataRange);
        }

        public void PublishRange(IEnumerable<byte[]> dataRange, string routingKey)
        {
            this.ManagedAction((x) =>  _publisher.PublishRange(x, routingKey), BehaviourScope.Message, dataRange);
        }

        public void PublishTuples(IEnumerable<Tuple<byte[], string>> tuples)
        {
            this.ManagedAction((x) =>  _publisher.PublishTuples(x), BehaviourScope.Message, tuples);
        }

        public virtual void Start()
        {
            this.ManagedAction(OnStart, BehaviourScope.Service);
        }

        protected abstract void OnStart();

        public virtual void Stop()
        {
            this.ManagedAction(OnStop, BehaviourScope.Service);
        }

        protected abstract void OnStop();
    }
}