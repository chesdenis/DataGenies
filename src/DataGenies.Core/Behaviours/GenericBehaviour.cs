using System;
using DataGenies.Core.Containers;
using DataGenies.Core.Models;
using DataGenies.Core.Roles;

namespace DataGenies.Core.Behaviours
{
    public abstract class GenericBehaviour<T> : IBehaviour
    {
        protected IContainer ContextContainer;

        public void SetContextContainer(IContainer container)
        {
            ContextContainer = container;
        }

        public virtual void DoSomethingBeforeStart()
        {
            throw new NotImplementedException();
        }

        public virtual void DoSomethingBeforeStart(byte[] message)
        {
            throw new NotImplementedException();
        }

        public virtual void DoSomethingAfterStart()
        {
            throw new NotImplementedException();
        }

        public virtual void DoSomethingAfterStart(byte[] message)
        {
            throw new NotImplementedException();
        }

        public virtual void DoSomethingDuringRunning(Action action)
        {
            throw new NotImplementedException();
        }

        public virtual void DoSomethingDuringRunning(Action<byte[]> action)
        {
            throw new NotImplementedException();
        }

        public abstract BehaviourType Type { get; set; }

       
        public Action Wrap(Action<Action> wrapperAction, Action executeAction)
        {
            return () => wrapperAction(executeAction);
        }

        public virtual Action<byte[]> Wrap(Action<Action<byte[]>> wrapperAction, Action<byte[]> executeAction)
        {
            throw new NotImplementedException();
        }

        public virtual void DoSomethingOnException(Exception ex = null)
        {
            throw new NotImplementedException();
        }

        public virtual void DoSomethingOnException(byte[] message, Exception ex = null)
        {
            throw new NotImplementedException();
        }
    }
}