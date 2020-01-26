using System;

namespace DataGenies.AspNetCore.DataGeniesCore.Abstractions
{
    public abstract class GenericBehaviour : IBehaviour
    {
        public abstract BehaviourType Type { get; set; }
       
        public abstract void ExecuteAction(Action action);

        public abstract void ExecuteException(Exception ex = null);

        public abstract void ExecuteScalar();

        public Action Wrap(Action<Action> wrapperAction, Action executeAction)
        {
            return () => wrapperAction(executeAction);
        }
    }
}