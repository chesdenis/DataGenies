using System;
using DataGenies.Core.Behaviours;
using DataGenies.Core.Containers;

namespace DataGenies.Core.Wrappers
{
    public interface IWrapperBehaviour : IBehaviour
    {
        Action Wrap(Action<Action> wrapperAction, Action executeAction);
        
        Action<T> Wrap<T>(WrappedTypedAction<T> wrapperAction, Action<T> executeAction) where T : class;
       
        void BehaviourAction(Action action);

        void BehaviourAction<T>(Action<T> action, T arg);

        Action<IContainer> Wrap(WrappedActionWithContainer wrapperAction, Action<IContainer> executeAction);

        void BehaviourAction(Action<IContainer> action, IContainer container);
    }
}