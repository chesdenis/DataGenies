namespace DG.Core.InMemoryMessageBroker
{
    using System;
    using System.Collections.Generic;
    using DG.Core.Model.Message;

    public class InMemoryPublisher : IPublisher
    {
        private readonly InMemoryMqBroker broker;

        public InMemoryPublisher(InMemoryMqBroker broker)
        {
            this.broker = broker;
        }

        public void Publish(string exchange, MqMessage message)
        {
            if (!this.broker.Model[exchange].ContainsKey(message.RoutingKey))
            {
                return;
            }

            var contextQueues = this.broker.Model[exchange][message.RoutingKey].ToArray();

            Array.ForEach(contextQueues, queue =>
            {
                queue.Enqueue(new MqMessage
                {
                    Body = message.Body,
                    RoutingKey = message.RoutingKey,
                });
            });
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
