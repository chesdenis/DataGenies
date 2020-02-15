using System;
using DataGenies.Core.Containers;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace DataGenies.Core.Behaviours
{
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
        Action Wrap(Action<Action> wrapperAction, Action executeAction);

        Action<T> Wrap<T>(Action<Action<T>> wrapperAction, Action<T> executeAction);
        
        void WrapAction(Action action);
        
        void WrapAction<T>(Action<T> action);

        T1 ChainFunction<T0, T1>(T0 arg);
    }

    public interface IBehaviorBeforeStart : IBasicBehaviour
    {
        
    }

    public interface IBehaviourAfterStart : IBasicBehaviour
    {
        
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