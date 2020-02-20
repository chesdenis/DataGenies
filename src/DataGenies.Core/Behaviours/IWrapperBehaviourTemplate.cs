using System;
using DataGenies.Core.Behaviours;
using DataGenies.Core.Containers;
using DataGenies.Core.Services;
using DataGenies.InMemory;

namespace DataGenies.Core.Wrappers
{
    public interface IWrapperBehaviourTemplate
    {
        public IManagedService ManagedService { get; set; }
        
        Action<T> WrapMessageHandling<T>(Action<Action<T>, T> behaviourActionWithMessage,
            Action<T> executeAction, T mqMessage) where T : MqMessage;

        Action<T> WrapContainerHandling<T>(Action<Action<T>, T> behaviourActionWithContainer,
            Action<T> executeAction, T mqMessage) where T : IContainer;
        
        void BehaviourActionWithMessage<T>(Action<T> action, T message) where T : MqMessage;

        void BehaviourActionWithContainer<T>(Action<T> action, T container) where T : IContainer;
    }
}