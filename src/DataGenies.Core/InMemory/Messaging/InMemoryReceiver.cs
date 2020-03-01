using System;
using System.Linq;
using DataGenies.Core.Models;
using DataGenies.Core.Receivers;

namespace DataGenies.Core.InMemory.Messaging
{
    public class InMemoryReceiver : IReceiver
    {
        private readonly InMemoryMqBroker _broker;
      
        private bool _isListening;

        public InMemoryReceiver(InMemoryMqBroker broker)
        {
            _broker = broker;
        }
        
        public void Listen(string queueName, Action<MqMessage> onReceive)
        {
            var relatedQueue = _broker.Model.Keys
                .SelectMany(ss => _broker.Model[ss])
                .SelectMany(ss => ss.Value).First(f => f.Name == queueName);
             
            this._isListening = true;
            while (_isListening)
            {
                if (!relatedQueue.TryDequeue(out var message)) continue;

                try
                {
                    onReceive(message);
                }
                catch (Exception)
                {
                    relatedQueue.Enqueue(message);
                }
            }
        }

        public void StopListen()
        {
            _isListening = false;
        }
    }
}