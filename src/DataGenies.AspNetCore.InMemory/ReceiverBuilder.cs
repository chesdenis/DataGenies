using System.Collections.Generic;
using DataGenies.AspNetCore.DataGeniesCore.Receivers;

namespace DataGenies.AspNetCore.InMemory
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

        public ReceiverBuilder WithQueue(string queueName)
        {
            this.QueueName = queueName;
            this.RoutingKeys = new[] {"#"};
            return this;
        }
        
        public ReceiverBuilder WithRoutingKeys(IEnumerable<string> routingKeys)
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