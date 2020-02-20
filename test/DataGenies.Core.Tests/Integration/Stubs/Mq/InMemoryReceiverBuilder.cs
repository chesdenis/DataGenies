using DataGenies.Core.Receivers;

namespace DataGenies.Core.Tests.Integration.Stubs.Mq
{
    public class InMemoryReceiverBuilder : IReceiverBuilder
    {
        private readonly InMemoryMqBroker _broker;
        protected string QueueName { get; set; }

        public InMemoryReceiverBuilder(InMemoryMqBroker broker)
        {
            _broker = broker;
        }

        public IReceiverBuilder WithQueue(string queueName)
        {
            this.QueueName = queueName;
            return this;
        }
        
        public IReceiver Build()
        {
            return new InMemoryReceiver(_broker, this.QueueName);
        }
    }
}