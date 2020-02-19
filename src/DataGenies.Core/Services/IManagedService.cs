using System.Collections.Generic;
using DataGenies.Core.Behaviours;
using DataGenies.Core.Wrappers;

namespace DataGenies.Core.Services
{
    public interface IManagedService : IRestartable
    {
        IEnumerable<BehaviourTemplate> BehaviourTemplates { get; }
        IEnumerable<WrapperBehaviourTemplate> WrapperBehaviours { get; }
    }
}