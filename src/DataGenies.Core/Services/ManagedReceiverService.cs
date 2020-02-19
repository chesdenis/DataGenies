using System;
using System.Collections.Generic;
using DataGenies.Core.Behaviours;
using DataGenies.Core.Extensions;
using DataGenies.Core.Receivers;
using DataGenies.Core.Wrappers;
using DataGenies.InMemory;

namespace DataGenies.Core.Services
{
    public abstract class ManagedReceiverService : ManagedService, IReceiver
    {
        private readonly IReceiver _receiver;
        
        protected ManagedReceiverService(
            IReceiver receiver,    
            IEnumerable<BehaviourTemplate> behaviourTemplates,
            IEnumerable<WrapperBehaviourTemplate> wrapperBehaviours)
            : base(behaviourTemplates, wrapperBehaviours)
        {
            _receiver = receiver;
        }

        
        public void Listen(Action<MqMessage> onReceive)
        {
            this.ManagedAction(() =>
            {
                _receiver.Listen(arg =>
                    this.ManagedActionWithMessage(onReceive, arg, BehaviourScope.Message));
            }, BehaviourScope.Service);
        }

        public void StopListen()
        {
            this.ManagedAction(() => _receiver.StopListen(), BehaviourScope.Service);
        }
    }
}