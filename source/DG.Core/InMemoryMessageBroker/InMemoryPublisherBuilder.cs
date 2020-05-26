namespace DG.Core.InMemoryMessageBroker
{
    public class InMemoryPublisherBuilder : IPublisherBuilder
    {
        private readonly InMemoryMqBroker broker;

        public InMemoryPublisherBuilder(InMemoryMqBroker broker)
        {
            this.broker = broker;
        }

        public IPublisher Build()
        {
            return new InMemoryPublisher(this.broker);
        }
    }
}
