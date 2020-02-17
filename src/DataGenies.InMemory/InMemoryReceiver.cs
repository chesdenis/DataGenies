using System;
using System.Collections.Generic;
using System.Linq;
using DataGenies.Core.Receivers;

namespace DataGenies.InMemory
{
    public class InMemoryReceiver : IReceiver
    {
        private readonly InMemoryMqBroker _broker;
        private readonly string _queueName;
         
        private bool _isListening;

        public InMemoryReceiver(InMemoryMqBroker broker, string queueName)
        {
            _broker = broker;
            _queueName = queueName;
        }

        public void Listen(Action<MqMessage> onReceive)
        {
            var relatedQueue = _broker.Model.Keys
                .SelectMany(ss => _broker.Model[ss])
                .SelectMany(ss => ss.Value).First(f => f.Name == _queueName);
             
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