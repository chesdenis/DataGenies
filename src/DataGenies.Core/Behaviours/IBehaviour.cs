using System;
using DataGenies.Core.Containers;

namespace DataGenies.Core.Behaviours
{
    public interface IBehaviour
    {
        BehaviourType Type { get; set; }

        void SetContextContainer(IContainer container);

        void DoSomethingBeforeStart();
        
        void DoSomethingAfterStart();

        void DoSomethingDuringRunning(Action action);

        Action Wrap(Action<Action> wrapperAction, Action executeAction);

        void DoSomethingOnException(Exception ex = null);
    }
}