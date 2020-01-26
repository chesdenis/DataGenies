using System;

namespace DataGenies.AspNetCore.DataGeniesCore.Abstraction
{
    public interface IReceiver
    {
        void Listen(Action<byte[]> onReceive);
    }
}