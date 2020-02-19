using System;
using System.Collections.Generic;
using DataGenies.Core.Behaviours;
using DataGenies.Core.Containers;
using DataGenies.Core.Extensions;
using DataGenies.Core.Publishers;
using DataGenies.Core.Wrappers;
using DataGenies.InMemory;

namespace DataGenies.Core.Services
{
    public abstract class ManagedPublisherServiceWithContainer : IPublisher, IManagedServiceWithContainer
    {
        private readonly IPublisher _publisher;
        public IEnumerable<BehaviourTemplate> BehaviourTemplates { get; }
        public IEnumerable<WrapperBehaviourTemplate> WrapperBehaviours { get; }

        protected ManagedPublisherServiceWithContainer(
            IContainer container,
            IPublisher publisher, 
            IEnumerable<BehaviourTemplate> behaviourTemplates,
            IEnumerable<WrapperBehaviourTemplate> wrapperBehaviours)
        {
            Container = container;
            
            _publisher = publisher;
            BehaviourTemplates = behaviourTemplates;
            WrapperBehaviours = wrapperBehaviours;
        }
        
        public void Publish(MqMessage data)
        {
            this.ManagedActionWithMessage(x =>_publisher.Publish(x), data, BehaviourScope.Message);
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

        public virtual void Start()
        {
            this.ManagedActionWithContainer((x) => OnStart(), Container, BehaviourScope.Service);
        }

        protected abstract void OnStart();

        public virtual void Stop()
        {
            this.ManagedActionWithContainer((x)=>OnStop(), Container, BehaviourScope.Service);
        }

        protected abstract void OnStop();

        public IContainer Container { get; }
    }
}