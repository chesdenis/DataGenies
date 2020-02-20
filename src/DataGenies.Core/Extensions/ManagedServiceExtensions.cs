using System;
using System.Linq;
using DataGenies.Core.Behaviours;
using DataGenies.Core.Containers;
using DataGenies.Core.Services;
using DataGenies.InMemory;

namespace DataGenies.Core.Extensions
{
    public static class ManagedServiceExtensions
    {
        public static void ManagedActionWithMessage<T>(this IManagedService managedService, Action<T> execute, T message, BehaviourScope behaviourScope) where T: MqMessage
        {
            try
            {
                foreach (var beforeStart in managedService.BehaviourTemplates
                    .Where(w=>w.BehaviourScope == behaviourScope && w.BehaviourType == BehaviourType.BeforeRun))
                {
                    beforeStart.Execute(message);
                }

                Action<T> resultAction = execute;

                foreach (var wrapper in managedService.WrapperBehaviours
                    .Where(w=>w.BehaviourScope == behaviourScope))
                {
                    resultAction = wrapper.WrapMessageHandling(wrapper.BehaviourActionWithMessage, resultAction, message);
                }

                resultAction(message);
            }
            catch (Exception ex)
            {
                foreach (var onException in managedService.BehaviourTemplates
                    .Where(w=>w.BehaviourScope == behaviourScope && w.BehaviourType == BehaviourType.OnException))
                {
                    onException.Execute(message, ex);
                }
            }
            finally
            {
                foreach (var afterStart in managedService.BehaviourTemplates
                    .Where(w=>w.BehaviourScope == behaviourScope && w.BehaviourType == BehaviourType.AfterRun))
                {
                    afterStart.Execute(message);
                }
            }
        }
        
        public static void ManagedActionWithContainer<T>(this IManagedService managedService, Action<T> execute, T container, BehaviourScope behaviourScope) where T: IContainer
        {
            try
            {
                foreach (var beforeStart in managedService.BehaviourTemplates
                    .Where(w=>w.BehaviourScope == behaviourScope && w.BehaviourType == BehaviourType.BeforeRun))
                {
                    beforeStart.Execute(container);
                }

                Action<T> resultAction = execute;

                foreach (var wrapper in managedService.WrapperBehaviours
                    .Where(w=>w.BehaviourScope == behaviourScope))
                {
                    resultAction = wrapper.WrapContainerHandling(wrapper.BehaviourActionWithContainer, resultAction, container);
                }

                resultAction(container);
            }
            catch (Exception ex)
            {
                foreach (var onException in managedService.BehaviourTemplates
                    .Where(w=>w.BehaviourScope == behaviourScope && w.BehaviourType == BehaviourType.OnException))
                {
                    onException.Execute(container, ex);
                }
            }
            finally
            {
                foreach (var afterStart in managedService.BehaviourTemplates
                    .Where(w=>w.BehaviourScope == behaviourScope && w.BehaviourType == BehaviourType.AfterRun))
                {
                    afterStart.Execute(container);
                }
            }
        }
    }
}