using System;
using DataGenies.Core.Containers;
using DataGenies.Core.Models;
using DataGenies.Core.Roles;

namespace DataGenies.Core.Behaviours
{
    public abstract class GenericBehaviour<T> : IBehaviour
    {
        protected IStateContainer StateContainer;

        public void SetStateContainer(IStateContainer stateContainer)
        {
            StateContainer = stateContainer;
        }
        
        public abstract BehaviourType Type { get; set; }

        public abstract void DoSomethingBeforeStart();
        
        public abstract void DoSomethingAfterStart();

        public abstract void DoSomethingDuringRunning(Action action);

        public abstract void DoSomethingOnException(Exception ex = null);
        
        public Action Wrap(Action<Action> wrapperAction, Action executeAction)
        {
            return () => wrapperAction(executeAction);
        }
    }
}