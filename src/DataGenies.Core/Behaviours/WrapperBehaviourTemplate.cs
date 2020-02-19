using System;
using DataGenies.Core.Containers;
using DataGenies.Core.Services;
using DataGenies.Core.Wrappers;
using DataGenies.InMemory;

namespace DataGenies.Core.Behaviours
{
    public abstract class WrapperBehaviourTemplate : IWrapperBehaviourTemplate 
    {
        public abstract BehaviourScope BehaviourScope { get; set; }

        public IManagedService ManagedService { get; set; }

        public Action Wrap(Action<Action> wrapperAction, Action executeAction)
        {
            return () => wrapperAction(executeAction);
        }

        public Action<T> WrapMessageHandling<T>(Action<Action<T>, T> behaviourActionWithMessage,
            Action<T> executeAction, T mqMessage) where T : MqMessage
        {
            return (message) => behaviourActionWithMessage(executeAction, message);
        }

        public Action<T> WrapContainerHandling<T>(Action<Action<T>, T> behaviourActionWithContainer,
            Action<T> executeAction, T mqMessage) where T : IContainer
        {
            return (container) => behaviourActionWithContainer(executeAction, container);
        }
 
        public virtual void BehaviourAction(Action action)
        {
            throw new NotImplementedException();
        }

        public virtual void BehaviourActionWithMessage<T>(Action<T> action, T message) where T : MqMessage
        {
            throw new NotImplementedException();
        }

        public virtual void BehaviourActionWithContainer<T>(Action<T> action, T container) where T : IContainer
        {
            throw new NotImplementedException();
        }
    }
}