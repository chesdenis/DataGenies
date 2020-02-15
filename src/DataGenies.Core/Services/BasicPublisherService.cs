using System;
using DataGenies.Core.Extensions;
using DataGenies.Core.Publishers;
using DataGenies.Core.Roles;
using DataGenies.Core.Stores;

namespace DataGenies.Core.Services
{
    public class BasicPublisherService : IRestartable
    {
        protected readonly IPublisher Publisher;
        protected readonly BehavioursStore BehavioursStore;
        protected readonly ConvertersStore ConvertersStore;

        public BasicPublisherService(IPublisher publisher, BehavioursStore behavioursStore, ConvertersStore convertersStore)
        {
            Publisher = publisher;
            BehavioursStore = behavioursStore;
            ConvertersStore = convertersStore;
        }
        
        public virtual void Start()
        {
            throw new NotImplementedException();
        }

        public virtual void Stop()
        {
            throw new NotImplementedException();
        }
    }

    public class NumbersPublisherService : BasicPublisherService
    {
       
        public override void Start()
        {
            for (int i = 0; i < 10; i++)
            {
                this.Publisher.Publish(i.ToBytes());
            }
        }
    }
}