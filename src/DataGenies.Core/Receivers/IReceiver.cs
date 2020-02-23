using System;
using DataGenies.Core.Models;

namespace DataGenies.Core.Receivers
{
    public interface IReceiver
    {
        void Listen(string queueName, Action<MqMessage> onReceive);

        void StopListen();
    }
}