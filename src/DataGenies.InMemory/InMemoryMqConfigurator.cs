using System.Collections.Generic;
using System.Linq;
using DataGenies.Core.Configurators;

namespace DataGenies.InMemory
{
    public class InMemoryMqConfigurator : IMqConfigurator
    {
        private readonly InMemoryMqBroker _broker;

        public InMemoryMqConfigurator(InMemoryMqBroker broker)
        {
            _broker = broker;
        }
        
        public void EnsureExchange(string exchangeName)
        {
            if (_broker.Model.ContainsKey(exchangeName))
            {
                return;
            }

            _broker.Model.TryAdd(exchangeName, new InMemoryRoutingKeysWithQueues());
        }

        public void EnsureQueue(string queueName, string exchangeName, string routingKey)
        {
            if (!_broker.Model[exchangeName].ContainsKey(routingKey))
            {
                _broker.Model[exchangeName].TryAdd(routingKey, new List<InMemoryQueue>());
            }

            var relatedQueues = _broker.Model[exchangeName][routingKey];

            if (relatedQueues.All(w => w.Name != queueName))
            {
                relatedQueues.Add(new InMemoryQueue() {Name = queueName});
            }
        }
    }
}