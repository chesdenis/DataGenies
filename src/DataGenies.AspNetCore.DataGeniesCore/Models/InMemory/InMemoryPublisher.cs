using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataGenies.AspNetCore.DataGeniesCore.Publishers;

namespace DataGenies.AspNetCore.DataGeniesCore.Models.InMemory
{
    public class InMemoryPublisher : IPublisher
    {
        private const string _routingKey = "#";
        
        private readonly InMemoryMqBroker _broker;
        private readonly string _exchangeName;

        private IEnumerable<Queue<InMemoryMqMessage>> ContextQueues
        {
            get
            {
                return this._broker.ExchangesAndBoundQueues
                    .Where(w => w.Item1 == _exchangeName)
                    .Select(s => s.Item2);
            }
        }

        public InMemoryPublisher(InMemoryMqBroker broker, string exchangeName)
        {
            _broker = broker;
            _exchangeName = exchangeName;
        }

        public void Publish(byte[] data) => this.Publish(data, _routingKey);
        
        public void Publish(byte[] data, string routingKey)
        {
            Array.ForEach(this.ContextQueues.ToArray(), queue =>
            {
                queue.Enqueue(new InMemoryMqMessage
                {
                    Body = data,
                    RoutingKey = routingKey
                });
            });
         
        }

        public void Publish(byte[] data, IEnumerable<string> routingKeys)
        {
            Array.ForEach(routingKeys.ToArray(), rk => { this.Publish(data, rk); });
        }

        public void PublishRange(IEnumerable<byte[]> dataRange)
        {
            Array.ForEach(dataRange.ToArray(), data => { this.Publish(data); });
        }

        public void PublishRange(IEnumerable<byte[]> dataRange, string routingKey)
        {
            Array.ForEach(dataRange.ToArray(), data => { this.Publish(data, routingKey); });
        }

        public void PublishTuples(IEnumerable<Tuple<byte[], string>> tuples)
        {
            throw new NotImplementedException();
        }
    }
}