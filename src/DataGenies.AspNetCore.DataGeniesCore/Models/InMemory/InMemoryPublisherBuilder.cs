using System;
using DataGenies.AspNetCore.DataGeniesCore.Publishers;

namespace DataGenies.AspNetCore.DataGeniesCore.Models.InMemory
{
    public class InMemoryPublisherBuilder : IPublisherBuilder
    {
        private readonly InMemoryMqBroker _broker;
        
        private string ExchangeName { get; set; }
        
        public InMemoryPublisherBuilder(InMemoryMqBroker broker)
        {
            _broker = broker;
        }
        
        public InMemoryPublisherBuilder WithExchange(string exchangeName)
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