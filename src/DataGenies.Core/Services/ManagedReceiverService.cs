using System;
using System.Collections.Generic;
using DataGenies.Core.Behaviours;
using DataGenies.Core.Extensions;
using DataGenies.Core.Receivers;
using DataGenies.Core.Wrappers;

namespace DataGenies.Core.Services
{
    public abstract class ManagedReceiverService : IReceiver, IRestartable, IManagedService
    {
        private readonly IReceiver _receiver;
        
        public IEnumerable<IBasicBehaviour> BasicBehaviours { get; }
        public IEnumerable<IBehaviourOnException> BehaviourOnExceptions { get; }
        public IEnumerable<IWrapperBehaviour> WrapperBehaviours { get; }

        protected ManagedReceiverService(IReceiver receiver,    
            IEnumerable<IBasicBehaviour> basicBehaviours,
            IEnumerable<IBehaviourOnException> behaviourOnExceptions,
            IEnumerable<IWrapperBehaviour> wrapperBehaviours)
        {
            _receiver = receiver;
            BasicBehaviours = basicBehaviours;
            BehaviourOnExceptions = behaviourOnExceptions;
            WrapperBehaviours = wrapperBehaviours;
        }

        public void Listen(Action<byte[]> onReceive)
        {
            _receiver.Listen(arg =>
                this.ManagedAction(() => { onReceive(arg); }, BehaviourScope.Message));
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