using System;
using DataGenies.Core.Containers;

namespace DataGenies.Core.Behaviours
{
    public interface IBehaviour
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