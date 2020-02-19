using System;
using System.Collections.Generic;
using DataGenies.Core.Behaviours;
using DataGenies.Core.Extensions;
using DataGenies.Core.Publishers;
using DataGenies.Core.Wrappers;
using DataGenies.InMemory;

namespace DataGenies.Core.Services
{
    public abstract class ManagedPublisherService : ManagedService, IPublisher 
    {
        private readonly IPublisher _publisher;

        protected ManagedPublisherService(
            IPublisher publisher,
            IEnumerable<BehaviourTemplate> behaviourTemplates,
            IEnumerable<WrapperBehaviourTemplate> wrapperBehaviours) 
            : base(behaviourTemplates, wrapperBehaviours)
        {
            _publisher = publisher;
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