using System;
using DataGenies.Core.Models;
using DataGenies.Core.Roles;

namespace DataGenies.Core.Behaviours
{
    public abstract class GenericBehaviour : IBehaviour
    {
        public abstract BehaviourType Type { get; set; }

        public abstract void DoSomethingBeforeStart();
        
        public abstract void DoSomethingAfterStart();

        public abstract void DoSomethingDuringRunning(Action action);

        public abstract void DoSomethingOnException(Exception ex = null);
        
        public Func<IApplicationProperties> GetApplicationProperties { get; set; }

        public Action Wrap(Action<Action> wrapperAction, Action executeAction)
        {
            return () => wrapperAction(executeAction);
        }
    }
}