using System;
using System.Collections.Generic;
using DataGenies.AspNetCore.DataGeniesCore.Receivers;

namespace DataGenies.AspNetCore.DataGeniesCore.Models.InMemory
{
    public class InMemoryReceiverBuilder : ReceiverBuilder
    {
        private readonly InMemoryMqBroker _broker;
        protected string QueueName { get; set; }
        protected IEnumerable<string> RoutingKeys { get; set; }

        public InMemoryReceiverBuilder(InMemoryMqBroker broker)
        {
            _broker = broker;
        }

        public InMemoryReceiverBuilder WithQueue(string queueName)
        {
            this.QueueName = queueName;
            this.RoutingKeys = new[] {"#"};
            return this;
        }
        
        public InMemoryReceiverBuilder WithRoutingKeys(IEnumerable<string> routingKeys)
        {
            this.RoutingKeys = routingKeys;
            return this;
        }
        
        public override IReceiver Build()
        {
            return new InMemoryReceiver(_broker, this.QueueName, this.RoutingKeys);
        }
    }
}