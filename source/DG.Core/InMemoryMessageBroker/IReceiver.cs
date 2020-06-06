namespace DG.Core.InMemoryMessageBroker
{
    using System;
    using DG.Core.Model.Message;

    public interface IReceiver
    {
        void Listen(string queueName, Action<MqMessage> onReceive);

        void StopListen();
    }
}
