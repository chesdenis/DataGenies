using DataGenies.AspNetCore.DataGeniesCore.Publishers;

namespace DataGenies.AspNetCore.InMemory
{
    public class PublisherBuilder : IPublisherBuilder
    {
        private readonly MqBroker _broker;
        
        private string ExchangeName { get; set; }
        
        public PublisherBuilder(MqBroker broker)
        {
            _broker = broker;
        }
        
        public PublisherBuilder WithExchange(string exchangeName)
        {
            this.ExchangeName = exchangeName;
            return this;
        }
        
        public IPublisher Build()
        {
            return new Publisher(_broker, ExchangeName);
        }
    }
}