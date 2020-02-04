using System;
using System.Collections.Generic;
using DataGenies.Core.Models;
using DataGenies.Core.Roles;

namespace DataGenies.Core.Behaviours
{
    public interface IBehaviour
    {
        BehaviourType Type { get; set; }

        void DoSomethingBeforeStart();
        
        void DoSomethingAfterStart();

        void DoSomethingDuringRunning(Action action);

        Action Wrap(Action<Action> wrapperAction, Action executeAction);

        void DoSomethingOnException(Exception ex = null);

        Func<IApplicationProperties> GetApplicationProperties { get; set; }
    }
}