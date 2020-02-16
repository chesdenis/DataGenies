﻿using System.Collections.Generic;
using DataGenies.Core.Behaviours;
using DataGenies.Core.Wrappers;

namespace DataGenies.Core.Services
{
    public interface IManagedService : IRestartable
    {
        IEnumerable<IBasicBehaviour> BasicBehaviours { get; }
        IEnumerable<IBehaviourOnException> BehaviourOnExceptions { get; }
        IEnumerable<IWrapperBehaviour> WrapperBehaviours { get; }
    }
}