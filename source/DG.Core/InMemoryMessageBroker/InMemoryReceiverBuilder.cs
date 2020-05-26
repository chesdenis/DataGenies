namespace DG.Core.InMemoryMessageBroker
{
    public class InMemoryReceiverBuilder : IReceiverBuilder
    {
        private readonly InMemoryMqBroker broker;

        protected string QueueName { get; set; }

        public InMemoryReceiverBuilder(InMemoryMqBroker broker)
        {
            this.broker = broker;
        }

        public IReceiver Build()
        {
            return new InMemoryReceiver(this.broker);
        }
    }
}
