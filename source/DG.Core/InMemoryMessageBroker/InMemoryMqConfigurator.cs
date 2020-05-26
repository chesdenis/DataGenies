namespace DG.Core.InMemoryMessageBroker
{
    using System.Collections.Generic;
    using System.Linq;

    public class InMemoryMqConfigurator : IMqConfigurator
    {
        private readonly InMemoryMqBroker broker;

        public InMemoryMqConfigurator(InMemoryMqBroker broker)
        {
            this.broker = broker;
        }

        public void EnsureExchange(string exchangeName)
        {
            if (this.broker.Model.ContainsKey(exchangeName))
            {
                return;
            }

            this.broker.Model.TryAdd(exchangeName, new InMemoryRoutingKeysWithQueues());
        }

        public void EnsureQueue(string queueName, string exchangeName, string routingKey)
        {
            if (!this.broker.Model[exchangeName].ContainsKey(routingKey))
            {
                this.broker.Model[exchangeName].TryAdd(routingKey, new List<InMemoryQueue>());
            }

            var relatedQueues = this.broker.Model[exchangeName][routingKey];

            if (relatedQueues.All(w => w.Name != queueName))
            {
                relatedQueues.Add(new InMemoryQueue() { Name = queueName });
            }
        }
    }
}
