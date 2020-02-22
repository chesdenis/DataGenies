using System;
using System.Collections.Generic;
using DataGenies.Core.Models;

namespace DataGenies.Core.Services
{
    public interface IReceiverManagedService
    {
        void Listen(Action<MqMessage> onReceive);
        
        void ListenScoped(IEnumerable<VirtualBinding> scope, Action<MqMessage> onReceive);
        
        void StopListen();
    }
}