using System;

namespace DataGenies.AspNetCore.DataGeniesCore.Abstractions.Receivers
{
    public class FakeReceiver : IReceiver
    {
        public void Listen(Action<byte[]> onReceive)
        {
            for (var i = 0; i < 10; i++)
            {
                onReceive?.Invoke(new [] {(byte) i});
            }
        }
    }
}