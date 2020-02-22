using System;
using DataGenies.Core.Models;

namespace DataGenies.Core.Receivers
{
    public interface IReceiver
    {
        void Listen(Action<MqMessage> onReceive);

        void Listen(string queueName, Action<MqMessage> onReceive);

        void StopListen();
    }
}