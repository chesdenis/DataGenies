using System;

namespace DataGenies.AspNetCore.DataGeniesCore.Abstractions.Receivers
{
    public interface IReceiver
    {
        void Listen(Action<byte[]> onReceive);
    }
}