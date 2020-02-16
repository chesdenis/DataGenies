﻿using System;
using System.Collections.Generic;
using DataGenies.Core.Behaviours;
using DataGenies.Core.Containers;
using DataGenies.Core.Extensions;
using DataGenies.Core.Receivers;
using DataGenies.Core.Wrappers;

namespace DataGenies.Core.Services
{
    public abstract class ManagedReceiverServiceWithContainer : IReceiver, IRestartable, IManagedService
    {
        protected readonly IContainer Container;
        
        private readonly IReceiver _receiver;
        
        public IEnumerable<IBasicBehaviour> BasicBehaviours { get; }
        public IEnumerable<IBehaviourOnException> BehaviourOnExceptions { get; }
        public IEnumerable<IWrapperBehaviour> WrapperBehaviours { get; }
        
        protected ManagedReceiverServiceWithContainer(
            IContainer container,
            IReceiver receiver,    
            IEnumerable<IBasicBehaviour> basicBehaviours,
            IEnumerable<IBehaviourOnException> behaviourOnExceptions,
            IEnumerable<IWrapperBehaviour> wrapperBehaviours)
        {
            Container = container;
            
            _receiver = receiver;
            BasicBehaviours = basicBehaviours;
            BehaviourOnExceptions = behaviourOnExceptions;
            WrapperBehaviours = wrapperBehaviours;
        }

        public void Listen(Action<byte[]> onReceive)
        {
            this.ManagedActionWithContainer(container =>
            {
                _receiver.Listen(message =>
                    this.ManagedActionWithMessage(messageOnExecute => { onReceive(messageOnExecute); }, message));
                
            }, Container);
        }

        public void StopListen()
        {
            this.ManagedActionWithContainer(container => _receiver.StopListen(), Container);
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