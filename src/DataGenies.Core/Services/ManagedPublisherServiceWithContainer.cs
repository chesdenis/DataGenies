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
    public abstract class ManagedPublisherServiceWithContainer : ManagedServiceWithContainer, IPublisher 
    {
        private readonly IPublisher _publisher;
        
        protected ManagedPublisherServiceWithContainer(
            IContainer container, 
            IPublisher publisher,
            IEnumerable<BehaviourTemplate> behaviourTemplates, 
            IEnumerable<WrapperBehaviourTemplate> wrapperBehaviours) 
            : base(container, behaviourTemplates, wrapperBehaviours)
        {
            _publisher = publisher;
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
    }
}