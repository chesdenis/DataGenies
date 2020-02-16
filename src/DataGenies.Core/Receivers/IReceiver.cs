using System;
using DataGenies.InMemory;

namespace DataGenies.Core.Receivers
{
    public interface IReceiver
    {
        void Listen(Action<MqMessage> onReceive);

        void StopListen();
    }
}