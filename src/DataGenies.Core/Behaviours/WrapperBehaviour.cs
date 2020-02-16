using System;
using DataGenies.Core.Containers;
using DataGenies.Core.Wrappers;

namespace DataGenies.Core.Behaviours
{
    public abstract class WrapperBehaviour  : IWrapperBehaviour 
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public abstract BehaviourScope BehaviourScope { get; set; }
        public abstract BehaviourType BehaviourType { get; set; }

        public Action Wrap(Action<Action> wrapperAction, Action executeAction)
        {
            return () => wrapperAction(executeAction);
        }

        public virtual void BehaviourAction(Action action)
        {
            throw new NotImplementedException();
        }

        public Action<IContainer> Wrap(WrappedActionWithContainer wrapperAction, Action<IContainer> executeAction)
        {
            return (arg) => wrapperAction(executeAction, arg);
        }

        public virtual void BehaviourAction(Action<IContainer> action, IContainer container)
        {
            throw new NotImplementedException();
        }
    }
}