using System;
using System.Collections.Generic;
using DataGenies.Core.Behaviours;
using DataGenies.Core.Extensions;
using DataGenies.Core.Publishers;
using DataGenies.Core.Wrappers;
using DataGenies.InMemory;

namespace DataGenies.Core.Services
{
    public abstract class ManagedPublisherService : IPublisher, IManagedService
    {
        private readonly IPublisher _publisher;
        public IEnumerable<BehaviourTemplate> BehaviourTemplates { get; }
        public IEnumerable<WrapperBehaviourTemplate> WrapperBehaviours { get; }

        protected ManagedPublisherService(
            IPublisher publisher, 
            IEnumerable<BehaviourTemplate> behaviourTemplates,
            IEnumerable<WrapperBehaviourTemplate> wrapperBehaviours)
        {
            _publisher = publisher;
            BehaviourTemplates = behaviourTemplates;
            WrapperBehaviours = wrapperBehaviours;
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