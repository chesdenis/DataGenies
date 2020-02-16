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
        // public static IContainer ManagedFunction(this IManagedService managedService, Func<IContainer, IContainer> function, IContainer arg, BehaviourScope behaviourScope)
        // {
        //     var retVal = default(IContainer);
        //     
        //     try
        //     {
        //         foreach (var beforeStart in managedService.BasicBehaviours.OfType<IBehaviorBeforeStart>())
        //         {
        //             beforeStart.Execute(arg);
        //         }
        //
        //         Func<IContainer, IContainer> resultFunction = function;
        //
        //         retVal = resultFunction(arg);
        //  
        //     }
        //     catch (Exception ex)
        //     {
        //         foreach (var onException in managedService.BehaviourOnExceptions)
        //         {
        //             onException.Execute(ex);
        //         }
        //     }
        //     finally
        //     {
        //         foreach (var afterStart in managedService.BasicBehaviours.OfType<IBehaviourAfterStart>())
        //         {
        //             afterStart.Execute();
        //         }
        //     }
        //
        //     return retVal;
        // }
        //
        // public static T ManagedFunction<T>(this IManagedService managedService, Func<T> function, BehaviourScope behaviourScope)
        // {
        //     var retVal = default(T);
        //     
        //     try
        //     {
        //         foreach (var beforeStart in managedService.BasicBehaviours.OfType<IBehaviorBeforeStart>())
        //         {
        //             beforeStart.Execute();
        //         }
        //
        //         retVal = function();
        //     }
        //     catch (Exception ex)
        //     {
        //         foreach (var onException in managedService.BehaviourOnExceptions)
        //         {
        //             onException.Execute(ex);
        //         }
        //     }
        //     finally
        //     {
        //         foreach (var afterStart in managedService.BasicBehaviours.OfType<IBehaviourAfterStart>())
        //         {
        //             afterStart.Execute();
        //         }
        //     }
        //
        //     return retVal;
        // }
        
        public static void ManagedAction(this IManagedService managedService, Action execute)
        {
            try
            {
                foreach (var beforeStart in managedService.BasicBehaviours.OfType<IBehaviorBeforeStart>())
                {
                    beforeStart.Execute();
                }

                Action resultAction = execute;

                foreach (var wrapper in managedService.WrapperBehaviours)
                {
                    resultAction = wrapper.Wrap(wrapper.BehaviourAction, resultAction);
                }

                resultAction();
            }
            catch (Exception ex)
            {
                foreach (var onException in managedService.BehaviourOnExceptions)
                {
                    onException.Execute(ex);
                }
            }
            finally
            {
                foreach (var afterStart in managedService.BasicBehaviours.OfType<IBehaviourAfterStart>())
                {
                    afterStart.Execute();
                }
            }
        }
        
        public static void ManagedActionWithMessage<T>(this IManagedService managedService, Action<T> execute, T arg) where T: MqMessage
        {
            try
            {
                foreach (var beforeStart in managedService.BasicBehaviours.OfType<IBehaviorBeforeStart>())
                {
                    beforeStart.Execute();
                }

                Action<T> resultAction = execute;

                foreach (var wrapper in managedService.WrapperBehaviours)
                {
                    resultAction = wrapper.WrapMessageHandling(wrapper.BehaviourActionWithMessage, resultAction, arg);
                }

                resultAction(arg);
            }
            catch (Exception ex)
            {
                foreach (var onException in managedService.BehaviourOnExceptions)
                {
                    onException.Execute(ex);
                }
            }
            finally
            {
                foreach (var afterStart in managedService.BasicBehaviours.OfType<IBehaviourAfterStart>())
                {
                    afterStart.Execute();
                }
            }
        }
        
        public static void ManagedActionWithContainer<T>(this IManagedService managedService, Action<T> execute, T arg) where T: IContainer
        {
            try
            {
                foreach (var beforeStart in managedService.BasicBehaviours.OfType<IBehaviorBeforeStart>())
                {
                    beforeStart.Execute();
                }

                Action<T> resultAction = execute;

                foreach (var wrapper in managedService.WrapperBehaviours)
                {
                    resultAction = wrapper.WrapContainerHandling(wrapper.BehaviourActionWithContainer, resultAction, arg);
                }

                resultAction(arg);
            }
            catch (Exception ex)
            {
                foreach (var onException in managedService.BehaviourOnExceptions)
                {
                    onException.Execute(ex);
                }
            }
            finally
            {
                foreach (var afterStart in managedService.BasicBehaviours.OfType<IBehaviourAfterStart>())
                {
                    afterStart.Execute();
                }
            }
        }
         
    }
}