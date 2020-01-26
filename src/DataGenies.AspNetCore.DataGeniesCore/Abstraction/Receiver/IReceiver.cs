using System;

namespace DataGenies.AspNetCore.DataGeniesCore.Abstraction.Receiver
{
    public interface IReceiver
    {
        void Listen(Action<byte[]> onReceive);
    }
}