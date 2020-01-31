using System;

namespace DataGenies.AspNetCore.DataGeniesCore.Receivers
{
    public interface IReceiver
    {
        void Listen(Action<byte[]> onReceive);

        void StopListen();
    }
}