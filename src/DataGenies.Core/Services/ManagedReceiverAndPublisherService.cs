using System;
using System.Collections.Generic;
using DataGenies.Core.Behaviours;
using DataGenies.Core.Extensions;
using DataGenies.Core.Publishers;
using DataGenies.Core.Receivers;
using DataGenies.Core.Wrappers;

namespace DataGenies.Core.Services
{
    public abstract class ManagedReceiverAndPublisherService : IPublisher, IReceiver, IManagedService, IRestartable
    {
        private readonly IPublisher _publisher;
        private readonly IReceiver _receiver;
        public IEnumerable<IBasicBehaviour> BasicBehaviours { get; }
        public IEnumerable<IBehaviourOnException> BehaviourOnExceptions { get; }
        public IEnumerable<IWrapperBehaviour> WrapperBehaviours { get; }
        
        protected ManagedReceiverAndPublisherService(
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
            this.ManagedAction(() =>_publisher.Publish(data), BehaviourScope.Message);
        }

        public void Publish(byte[] data, string routingKey)
        {
            this.ManagedAction(() =>_publisher.Publish(data, routingKey), BehaviourScope.Message);
        }

        public void Publish(byte[] data, IEnumerable<string> routingKeys)
        {
            this.ManagedAction(() =>_publisher.Publish(data, routingKeys), BehaviourScope.Message);
        }

        public void PublishRange(IEnumerable<byte[]> dataRange)
        {
            this.ManagedAction(() =>_publisher.PublishRange(dataRange), BehaviourScope.Message);
        }

        public void PublishRange(IEnumerable<byte[]> dataRange, string routingKey)
        {
            this.ManagedAction(() =>_publisher.PublishRange(dataRange, routingKey), BehaviourScope.Message);
        }

        public void PublishTuples(IEnumerable<Tuple<byte[], string>> tuples)
        {
            this.ManagedAction(() =>_publisher.PublishTuples(tuples), BehaviourScope.Message);
        }

        public void Listen(Action<byte[]> onReceive)
        {
            _receiver.Listen(arg =>
                this.ManagedAction(() => { onReceive(arg); }, BehaviourScope.Message));
        }

        public void StopListen()
        {
            this.ManagedAction(() => _receiver.StopListen(), BehaviourScope.Service);
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