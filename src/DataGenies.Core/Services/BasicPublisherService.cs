using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DataGenies.Core.Behaviours;
using DataGenies.Core.Extensions;
using DataGenies.Core.Publishers;
using DataGenies.Core.Roles;
using DataGenies.Core.Stores;

namespace DataGenies.Core.Services
{
    public interface IManagedService
    {
        IEnumerable<IBasicBehaviour> BasicBehaviours { get; }
        IEnumerable<IBehaviourOnException> BehaviourOnExceptions { get; }
        IEnumerable<IWrapperBehaviour> WrapperBehaviours { get; }
        IEnumerable<IConverterBehaviour> ConverterBehaviours { get; }
    }

    public static class ManagedServiceExtensions
    {
        public static T1 ManagedFunction<T0, T1>(this IManagedService managedService, Func<T0, T1> function, T0 arg)
        {
            var retVal = default(T1);
            
            try
            {
                foreach (var beforeStart in managedService.BasicBehaviours.OfType<IBehaviorBeforeStart>())
                {
                    beforeStart.Execute(arg);
                }

                Func<T0, T1> resultFunction = function;

                retVal = resultFunction(arg);

                foreach (var wrapper in managedService.WrapperBehaviours)
                {
                    retVal = wrapper.ChainFunction<T0, T1>(arg);
                }
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
        
        public static void ManagedAction<T>(this IManagedService managedService, Action<T> action, T arg)
        {
            try
            {
                foreach (var beforeStart in managedService.BasicBehaviours.OfType<IBehaviorBeforeStart>())
                {
                    beforeStart.Execute(arg);
                }

                Action<T> resultAction = action;

                foreach (var wrapper in managedService.WrapperBehaviours)
                {
                    resultAction = wrapper.Wrap(wrapper.WrapAction, resultAction);
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
                    resultAction = wrapper.Wrap(wrapper.WrapAction, resultAction);
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

    public class ManagedPublisherService : IRestartable, IManagedService
    {
        protected readonly IPublisher Publisher;
        public IEnumerable<IBasicBehaviour> BasicBehaviours { get; }
        public IEnumerable<IBehaviourOnException> BehaviourOnExceptions { get; }
        public IEnumerable<IWrapperBehaviour> WrapperBehaviours { get; }
        
        public IEnumerable<IConverterBehaviour> ConverterBehaviours { get; }
        
        public ManagedPublisherService(
            IPublisher publisher, 
            IEnumerable<IBasicBehaviour> basicBehaviours,
            IEnumerable<IBehaviourOnException> behaviourOnExceptions,
            IEnumerable<IWrapperBehaviour> wrapperBehaviours)
        {
            Publisher = publisher;
            
            BasicBehaviours = basicBehaviours;
            BehaviourOnExceptions = behaviourOnExceptions;
            WrapperBehaviours = wrapperBehaviours;
        }
        
        public virtual void Start()
        {
            throw new NotImplementedException();
        }
         
        public virtual void Stop()
        {
            throw new NotImplementedException();
        }
    }

    public class NumbersPublisherService : ManagedPublisherService
    {
        public NumbersPublisherService(IPublisher publisher, 
            IEnumerable<IBasicBehaviour> basicBehaviours,
            IEnumerable<IBehaviourOnException> behaviourOnExceptions, 
            IEnumerable<IWrapperBehaviour> wrapperBehaviours)
            : base(publisher, basicBehaviours, behaviourOnExceptions, wrapperBehaviours)
        {
        }

        public override void Start()
        {
            for (int i = 0; i < 10; i++)
            {
                this.ManagedAction((arg) =>
                {
                    this.Publisher.Publish(i.ToBytes());
                }, i.ToBytes());
            }
        }
    }
 
    public class SaveToDeepStorageBehaviour  :  WrapperBehaviour<byte[]>
    {
        public override void WrapAction(Action<byte[]> action, byte[] arg)
        {
            File.WriteAllBytes("abcde",  arg);
            
            action(arg);
        }
    }

    public class ClassA
    {
        public string A { get; set; }
    }

    public class ClassB
    {
        public string B { get; set; }
    }

    public class ConverterBehaviour : ConverterBehaviour<ClassA, ClassB>
    {
        public override ClassB ChainFunction(ClassA arg)
        {
            return new ClassB()
            {
                B = arg.A
            };
        }
    }
}