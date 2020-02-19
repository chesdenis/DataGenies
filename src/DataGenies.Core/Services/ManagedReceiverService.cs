using System;
using System.Collections.Generic;
using DataGenies.Core.Behaviours;
using DataGenies.Core.Extensions;
using DataGenies.Core.Receivers;
using DataGenies.Core.Wrappers;
using DataGenies.InMemory;

namespace DataGenies.Core.Services
{
    public abstract class ManagedReceiverService : IReceiver, IRestartable, IManagedService
    {
        private readonly IReceiver _receiver;
        
        public IEnumerable<BehaviourTemplate> BehaviourTemplates { get; }
        public IEnumerable<WrapperBehaviourTemplate> WrapperBehaviours { get; }

        protected ManagedReceiverService(
            IReceiver receiver,    
            IEnumerable<BehaviourTemplate> behaviourTemplates,
            IEnumerable<WrapperBehaviourTemplate> wrapperBehaviours)
        {
            _receiver = receiver;
            BehaviourTemplates = behaviourTemplates;
            WrapperBehaviours = wrapperBehaviours;
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

        public virtual void Start()
        {
            this.ManagedAction(OnStart, BehaviourScope.Service);
        }

        protected abstract void OnStart();

        public virtual void Stop()
        {
            this.ManagedAction(OnStop, BehaviourScope.Service);
        }

        protected abstract void OnStop();
    }
}