using System;
using DataGenies.Core.Containers;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace DataGenies.Core.Behaviours
{
    public class BasicBehaviour : IBasicBehaviour
    {
        public virtual void Execute()
        {
            throw new NotImplementedException();
        }

        public virtual void Execute<T>(T arg)
        {
            throw new NotImplementedException();
        }
    }

    public class BehaviourOnException : IBehaviourOnException
    {
        public void Execute(Exception exception)
        {
            throw new NotImplementedException();
        }

        public void Execute<T>(T arg, Exception exception)
        {
            throw new NotImplementedException();
        }
    }

    public interface IBasicBehaviour
    {
        void Execute();

        void Execute<T>(T arg);
    }

    public interface IBehaviourOnException
    {
        void Execute(Exception exception);

        void Execute<T>(T arg, Exception exception);
    }

    public interface IWrapperBehaviour
    {
        
    }

    public interface IWrapperBehaviour<T>
    {
        Action Wrap(Action<Action> wrapperAction, Action executeAction);

        Action<T> Wrap(Action<Action<T>> wrapperAction, Action<T> executeAction);
        
        void WrapAction(Action action);
        
        void WrapAction(Action<T> action, T arg);
    }

    public interface IConverterBehaviour
    {
        
    }

    public interface IConverterBehaviour<in T0, out T1>
    {
        T1 ChainFunction(T0 arg);
    }

    public abstract class ConverterBehaviour<T0, T1> : IConverterBehaviour<T0, T1>
    {
        public virtual T1 ChainFunction(T0 arg)
        {
            throw new NotImplementedException();
        }
    }

    public abstract class WrapperBehaviour<T> : IWrapperBehaviour<T>
    {
        public Action Wrap(Action<Action> wrapperAction, Action executeAction)
        {
            return () => wrapperAction(executeAction);
        }
    
        public Action<T> Wrap(Action<Action<T>> wrapperAction, Action<T> executeAction)
        {
            return (arg) => wrapperAction(executeAction);
        }
    
        public virtual void WrapAction(Action action)
        {
            action();
        }
    
        public virtual void WrapAction(Action<T> action, T arg)
        {
            action(arg);
        }
    }
    
    public interface IBehaviorBeforeStart : IBasicBehaviour
    {
        
    }

    public class BehaviorBeforeStart : BasicBehaviour
    {
        public override void Execute()
        {
            throw new NotImplementedException();
        }

        public override void Execute<T>(T arg)
        {
            throw new NotImplementedException();
        }
    }

    public interface IBehaviourAfterStart : IBasicBehaviour
    {
        
    }

    public class BehaviourAfterStart : BasicBehaviour
    {
        public override void Execute()
        {
            throw new NotImplementedException();
        }

        public override void Execute<T>(T arg)
        {
            throw new NotImplementedException();
        }
    }

    public interface IBehaviour_Old
    {
        BehaviourType Type { get; set; }

        void SetContextContainer(IContainer container);

        void DoSomethingBeforeStart();

        void DoSomethingBeforeStart(byte[] message);
        
        void DoSomethingAfterStart();

        void DoSomethingAfterStart(byte[] message);

        void DoSomethingDuringRunning(Action action);

        void DoSomethingDuringRunning(Action<byte[]> action);

        Action Wrap(Action<Action> wrapperAction, Action executeAction);

        Action<byte[]> Wrap(Action<Action<byte[]>> wrapperAction, Action<byte[]> executeAction);

        void DoSomethingOnException(Exception ex = null);

        void DoSomethingOnException(byte[] message, Exception ex = null);
    }
}