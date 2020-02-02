using System;
using System.Collections.Generic;
using DataGenies.Core.Receivers;

namespace DataGenies.InMemory
{
    public class ReceiverBuilder : IReceiverBuilder
    {
        private readonly MqBroker _broker;
        protected string QueueName { get; set; }

        public ReceiverBuilder(MqBroker broker)
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
            return new Receiver(_broker, this.QueueName);
        }
    }
}