using System;
using System.Collections.Generic;
using DataGenies.Core.Models;
using DataGenies.Core.Publishers;

namespace DataGenies.Core.Tests.Integration.Stubs.Mq
{
    public class InMemoryPublisher : IPublisher
    {
        private readonly InMemoryMqBroker _broker;
        private readonly string _exchangeName;
        
        public InMemoryPublisher(InMemoryMqBroker broker, string exchangeName)
        {
            _broker = broker;
            _exchangeName = exchangeName;
        }
        
        public void Publish(MqMessage message)
        {
            this.Publish(_exchangeName, message);
        }

        public void Publish(string exchange, MqMessage message)
        {
            if (!this._broker.Model[exchange].ContainsKey(message.RoutingKey))
            {
                return;
            }
            
            var contextQueues = this._broker.Model[exchange][message.RoutingKey].ToArray();
            
            Array.ForEach(contextQueues, queue =>
            {
                queue.Enqueue(new MqMessage
                {
                    Body = message.Body,
                    RoutingKey = message.RoutingKey
                });
            });
        }

        public void PublishRange(IEnumerable<MqMessage> dataRange)
        {
            foreach (var dataEntry in dataRange)
            {
                this.Publish(dataEntry);
            }
        }

        public void PublishRange(string exchange, IEnumerable<MqMessage> dataRange)
        {
            foreach (var dataEntry in dataRange)
            {
                this.Publish(exchange, dataEntry);
            }
        }
    }
}