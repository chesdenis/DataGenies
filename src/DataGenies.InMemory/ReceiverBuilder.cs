using System.Collections.Generic;
using DataGenies.Core.Receivers;

namespace DataGenies.InMemory
{
    public class ReceiverBuilder : IReceiverBuilder
    {
        private readonly MqBroker _broker;
        protected string QueueName { get; set; }
        protected IEnumerable<string> RoutingKeys { get; set; }

        public ReceiverBuilder(MqBroker broker)
        {
            _broker = broker;
        }

        public IReceiverBuilder WithQueue(string queueName)
        {
            this.QueueName = queueName;
            this.RoutingKeys = new[] {"#"};
            return this;
        }
        
        public IReceiverBuilder WithRoutingKeys(IEnumerable<string> routingKeys)
        {
            this.RoutingKeys = routingKeys;
            return this;
        }
        
        public IReceiver Build()
        {
            return new Receiver(_broker, this.QueueName, this.RoutingKeys);
        }
    }
}