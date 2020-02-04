using System;
using System.Collections.Generic;
using System.Linq;
using DataGenies.Core.Receivers;

namespace DataGenies.InMemory
{
    public class Receiver : IReceiver
    {
        private readonly MqBroker _broker;
        private readonly string _queueName;
         
        private bool _isListening = false;

        public Receiver(MqBroker broker, string queueName)
        {
            _broker = broker;
            _queueName = queueName;
        }

        public void Listen(Action<byte[]> onReceive)
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
                    onReceive(message.Body);
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