using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using DataGenies.Core.Publishers;

namespace DataGenies.InMemory
{
    public class Publisher : IPublisher
    {
        private const string _routingKey = "#";
        
        private readonly MqBroker _broker;
        private readonly string _exchangeName;
        
        public Publisher(MqBroker broker, string exchangeName)
        {
            _broker = broker;
            _exchangeName = exchangeName;
        }

        public void Publish(byte[] data) => this.Publish(data, _routingKey);
        
        public void Publish(byte[] data, string routingKey)
        {
            var contextQueues = this._broker.Model[_exchangeName][routingKey].ToArray();
            
            Array.ForEach(contextQueues, queue =>
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