using System.Collections.Generic;
using DataGenies.Core.Behaviours;
using DataGenies.Core.Containers;
using DataGenies.Core.Publishers;
using DataGenies.Core.Receivers;

namespace DataGenies.Core.Services
{
    public interface IManagedService : IRestartable, IPublisher, IReceiver
    {
        IContainer Container { get; }
        IEnumerable<BehaviourTemplate> BehaviourTemplates { get; }
        IEnumerable<WrapperBehaviourTemplate> WrapperBehaviours { get; }
    }
}