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
    public abstract class ManagedReceiverServiceWithContainer : IReceiver, IManagedServiceWithContainer
    {
        private readonly IReceiver _receiver;
        
        public IEnumerable<BehaviourTemplate> BehaviourTemplates { get; }
        public IEnumerable<WrapperBehaviourTemplate> WrapperBehaviours { get; }
        
        protected ManagedReceiverServiceWithContainer(
            IContainer container,
            IReceiver receiver,    
            IEnumerable<BehaviourTemplate> behaviourTemplates,
            IEnumerable<WrapperBehaviourTemplate> wrapperBehaviours)
        {
            Container = container;
            
            _receiver = receiver;
            BehaviourTemplates = behaviourTemplates;
            WrapperBehaviours = wrapperBehaviours;
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

        public virtual void Start()
        {
            this.ManagedActionWithContainer((x) => OnStart(), Container, BehaviourScope.Service);
        }

        protected abstract void OnStart();

        public virtual void Stop()
        {
            this.ManagedActionWithContainer((x) => OnStop(), Container, BehaviourScope.Service);
        }

        protected abstract void OnStop();
        public IContainer Container { get; }
    }
}