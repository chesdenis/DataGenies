using DataGenies.Core.Publishers;

namespace DataGenies.Core.Tests.Integration.Stubs.Mq
{
    public class InMemoryPublisherBuilder : IPublisherBuilder
    {
        private readonly InMemoryMqBroker _broker;
        
        public InMemoryPublisherBuilder(InMemoryMqBroker broker)
        {
            _broker = broker;
        }
        
        public IPublisher Build()
        {
            return new InMemoryPublisher(_broker);
        }
    }
}