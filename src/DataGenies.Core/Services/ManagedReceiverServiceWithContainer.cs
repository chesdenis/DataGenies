using System;
using System.Collections.Generic;
using DataGenies.Core.Behaviours;
using DataGenies.Core.Containers;
using DataGenies.Core.Extensions;
using DataGenies.Core.Receivers;
using DataGenies.Core.Wrappers;
using DataGenies.InMemory;

namespace DataGenies.Core.Services
{
    public abstract class ManagedReceiverServiceWithContainer : ManagedServiceWithContainer, IReceiver
    {
        private readonly IReceiver _receiver;
        
        protected ManagedReceiverServiceWithContainer(
            IContainer container,
            IReceiver receiver,    
            IEnumerable<BehaviourTemplate> behaviourTemplates,
            IEnumerable<WrapperBehaviourTemplate> wrapperBehaviours)
            : base(container, behaviourTemplates, wrapperBehaviours)
        {
            _receiver = receiver;
        }

        public void Listen(Action<MqMessage> onReceive)
        {
            this.ManagedActionWithContainer(container =>
            {
                _receiver.Listen(arg =>
                    this.ManagedActionWithMessage(onReceive, arg, BehaviourScope.Message));
                
            }, Container, BehaviourScope.Service);
        }

        public void StopListen()
        {
            this.ManagedActionWithContainer(container => _receiver.StopListen(), Container, BehaviourScope.Service);
        }
    }
}