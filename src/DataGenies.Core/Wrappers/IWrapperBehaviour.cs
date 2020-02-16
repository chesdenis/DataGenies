using System;
using DataGenies.Core.Behaviours;
using DataGenies.Core.Containers;

namespace DataGenies.Core.Wrappers
{
    public interface IWrapperBehaviour : IBehaviour
    {
        Action Wrap(Action<Action> wrapperAction, Action executeAction);
       
        void BehaviourAction(Action action);

        Action<IContainer> Wrap(WrappedActionWithContainer wrapperAction, Action<IContainer> executeAction);

        void BehaviourAction(Action<IContainer> action, IContainer container);
    }
}