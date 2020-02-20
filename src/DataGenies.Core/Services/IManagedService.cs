using System.Collections.Generic;
using DataGenies.Core.Behaviours;

namespace DataGenies.Core.Services
{
    public interface IManagedService : IRestartable
    {
        IEnumerable<BehaviourTemplate> BehaviourTemplates { get; }
        IEnumerable<WrapperBehaviourTemplate> WrapperBehaviours { get; }
    }
}