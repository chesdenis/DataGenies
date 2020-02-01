using System;

namespace DataGenies.Core.Receivers
{
    public interface IReceiver
    {
        void Listen(Action<byte[]> onReceive);

        void StopListen();
    }
}