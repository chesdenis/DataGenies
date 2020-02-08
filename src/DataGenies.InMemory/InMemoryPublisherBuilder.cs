using System;
using System.Runtime.Versioning;
using DataGenies.Core.Publishers;

namespace DataGenies.InMemory
{
    public class InMemoryPublisherBuilder : IPublisherBuilder
    {
        private readonly InMemoryMqBroker _broker;
        
        private string ExchangeName { get; set; }
        
        public InMemoryPublisherBuilder(InMemoryMqBroker broker)
        {
            _broker = broker;
        }
        
        public IPublisherBuilder WithExchange(string exchangeName)
        {
            this.ExchangeName = exchangeName;
            return this;
        }
        
        public IPublisher Build()
        {
            return new InMemoryPublisher(_broker, ExchangeName);
        }
    }
}