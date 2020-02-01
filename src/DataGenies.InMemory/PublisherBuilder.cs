using DataGenies.Core.Publishers;

namespace DataGenies.InMemory
{
    public class PublisherBuilder : IPublisherBuilder
    {
        private readonly MqBroker _broker;
        
        private string ExchangeName { get; set; }
        
        public PublisherBuilder(MqBroker broker)
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
            return new Publisher(_broker, ExchangeName);
        }
    }
}