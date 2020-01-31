using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataGenies.AspNetCore.DataGeniesCore.Receivers;

namespace DataGenies.AspNetCore.DataGeniesCore.Models.InMemory
{
    public class InMemoryReceiver : IReceiver
    {
        private readonly InMemoryMqBroker _broker;
        private readonly string _queueName;
        private readonly IEnumerable<string> _routingKeys;

        private bool _isListening = false;

        public InMemoryReceiver(InMemoryMqBroker broker, string queueName, IEnumerable<string> routingKeys)
        {
            _broker = broker;
            _queueName = queueName;
            _routingKeys = routingKeys;
        }

        public void Listen(Action<byte[]> onReceive)
        {
            var relatedQueue = _broker.ExchangesAndBoundQueues.First(f => f.Item2.Name == _queueName).Item2;

            this._isListening = true;
            while (_isListening)
            {
                Task.Delay(new TimeSpan(0, 0, 1)).Wait();

                if (!relatedQueue.TryDequeue(out var message)) continue;
                
                if (IsMatchForReceiver(message))
                {
                    onReceive(message.Body);
                    continue;
                }
                    
                relatedQueue.Enqueue(message);
            }
        }

        public void StopListen()
        {
            _isListening = false;
        }

        private bool IsMatchForReceiver(InMemoryMqMessage message)
        {
            foreach (var routingKey in this._routingKeys)
            {
                if (message.RoutingKey.StartsWith(routingKey))
                {
                    return true;
                }
            }

            return false;
        }
    }
}