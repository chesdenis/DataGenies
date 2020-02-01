using System;
using DataGenies.Core.Receivers;

namespace DataGenies.Core.Roles
{
    public abstract class ApplicationReceiverRole : IReceiver, IStartable
    {
        private readonly DataReceiverRole _receiverRole;

        public ApplicationReceiverRole(DataReceiverRole receiverRole)
        {
            _receiverRole = receiverRole;
        }

        public void Listen(Action<byte[]> onReceive)
        {
            _receiverRole.Listen(onReceive);
        }

        public void StopListen()
        {
            _receiverRole.StopListen();
        }

        public abstract void Start();
    }
}