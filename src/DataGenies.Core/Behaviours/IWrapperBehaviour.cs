using System;
using DataGenies.Core.Behaviours;
using DataGenies.Core.Containers;
using DataGenies.InMemory;

namespace DataGenies.Core.Wrappers
{
    public interface IWrapperBehaviour : IBehaviour
    {
        Action Wrap(Action<Action> wrapperAction, Action executeAction);

        Action<T> WrapMessageHandling<T>(Action<Action<T>, T> behaviourActionWithMessage,
            Action<T> executeAction, T mqMessage) where T : MqMessage;

        Action<T> WrapContainerHandling<T>(Action<Action<T>, T> behaviourActionWithContainer,
            Action<T> executeAction, T mqMessage) where T : IContainer;

        void BehaviourAction(Action action);

        void BehaviourActionWithMessage<T>(Action<T> action, T message) where T : MqMessage;

        void BehaviourActionWithContainer<T>(Action<T> action, T container) where T : IContainer;
    }
}