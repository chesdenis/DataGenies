namespace DG.Core.InMemoryMessageBroker
{
    using System;
    using System.Linq;
    using DG.Core.Model.Message;

    public class InMemoryReceiver : IReceiver
    {
        private readonly InMemoryMqBroker broker;

        private bool isListening;

        public InMemoryReceiver(InMemoryMqBroker broker)
        {
            this.broker = broker;
        }

        public void Listen(string queueName, Action<MqMessage> onReceive)
        {
            var relatedQueue = this.broker.Model.Keys
                .SelectMany(ss => this.broker.Model[ss])
                .SelectMany(ss => ss.Value).First(f => f.Name == queueName);

            this.isListening = true;
            while (this.isListening)
            {
                if (!relatedQueue.TryDequeue(out var message))
                {
                    continue;
                }

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
            this.isListening = false;
        }
    }
}
