using System;

namespace DataGenies.AspNetCore.DataGeniesCore.Abstractions
{
    public interface IReceiver
    {
        void Listen(Action<byte[]> onReceive);
    }
}