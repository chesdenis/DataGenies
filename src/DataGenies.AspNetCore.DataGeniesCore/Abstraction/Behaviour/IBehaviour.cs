﻿using System;

namespace DataGenies.AspNetCore.DataGeniesCore.Abstraction.Behaviour
{
    public interface IBehaviour
    {
        BehaviourType Type { get; set; }

        void ExecuteScalar();

        void ExecuteAction(Action action);

        Action Wrap(Action<Action> wrapperAction, Action executeAction);

        void ExecuteException(Exception ex = null);
    }
}