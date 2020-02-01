using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using DataGenies.AspNetCore.DataGeniesCore.Publishers;

namespace DataGenies.AspNetCore.InMemory
{
    public class Publisher : IPublisher
    {
        private const string _routingKey = "#";
        
        private readonly MqBroker _broker;
        private readonly string _exchangeName;

        private IEnumerable<ConcurrentQueue<MqMessage>> ContextQueues
        {
            get
            {
                return this._broker.ExchangesAndBoundQueues
                    .Where(w => w.Item1 == _exchangeName)
                    .Select(s => s.Item2);
            }
        }

        public Publisher(MqBroker broker, string exchangeName)
        {
            _broker = broker;
            _exchangeName = exchangeName;
        }

        public void Publish(byte[] data) => this.Publish(data, _routingKey);
        
        public void Publish(byte[] data, string routingKey)
        {
            Array.ForEach(this.ContextQueues.ToArray(), queue =>
            {
                queue.Enqueue(new MqMessage
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