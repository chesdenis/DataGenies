using System;
using System.Linq;
using DataGenies.Core.Behaviours;
using DataGenies.Core.Containers;
using DataGenies.Core.Services;

namespace DataGenies.Core.Extensions
{
    public static class ManagedServiceExtensions
    {
        public static IContainer ManagedFunction(this IManagedService managedService, Func<IContainer, IContainer> function, IContainer arg)
        {
            var retVal = default(IContainer);
            
            try
            {
                foreach (var beforeStart in managedService.BasicBehaviours.OfType<IBehaviorBeforeStart>())
                {
                    beforeStart.Execute(arg);
                }
        
                Func<IContainer, IContainer> resultFunction = function;
        
                retVal = resultFunction(arg);
         
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
        
            return retVal;
        }
        
        public static T ManagedFunction<T>(this IManagedService managedService, Func<T> function)
        {
            var retVal = default(T);
            
            try
            {
                foreach (var beforeStart in managedService.BasicBehaviours.OfType<IBehaviorBeforeStart>())
                {
                    beforeStart.Execute();
                }
        
                retVal = function();
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
        
            return retVal;
        }
        
        public static void ManagedAction(this IManagedService managedService, Action<IContainer> action, IContainer arg)
        {
            try
            {
                foreach (var beforeStart in managedService.BasicBehaviours.OfType<IBehaviorBeforeStart>())
                {
                    beforeStart.Execute(arg);
                }

                Action<IContainer> resultAction = action;

                foreach (var wrapper in managedService.WrapperBehaviours)
                {
                    resultAction = wrapper.Wrap(wrapper.BehaviourAction, resultAction);
                }

                resultAction(arg);
            }
            catch (Exception ex)
            {
                foreach (var onException in managedService.BehaviourOnExceptions)
                {
                    onException.Execute(arg, ex);
                }
            }
            finally
            {
                foreach (var afterStart in managedService.BasicBehaviours.OfType<IBehaviourAfterStart>())
                {
                    afterStart.Execute(arg);
                }
            }
        }

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
    }
}