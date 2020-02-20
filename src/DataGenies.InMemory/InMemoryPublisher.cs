using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using DataGenies.Core.Models;
using DataGenies.Core.Publishers;

namespace DataGenies.InMemory
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
            if (!this._broker.Model[_exchangeName].ContainsKey(message.RoutingKey))
            {
                return;
            }
            
            var contextQueues = this._broker.Model[_exchangeName][message.RoutingKey].ToArray();
            
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
    }
}