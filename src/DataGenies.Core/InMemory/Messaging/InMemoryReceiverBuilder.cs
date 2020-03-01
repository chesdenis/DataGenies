using DataGenies.Core.Receivers;

namespace DataGenies.Core.InMemory.Messaging
{
    public class InMemoryReceiverBuilder : IReceiverBuilder
    {
        private readonly InMemoryMqBroker _broker;
        protected string QueueName { get; set; }

        public InMemoryReceiverBuilder(InMemoryMqBroker broker)
        {
            _broker = broker;
        }
        
        public IReceiver Build()
        {
            return new InMemoryReceiver(_broker);
        }
    }
}