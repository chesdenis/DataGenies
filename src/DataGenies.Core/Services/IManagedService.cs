using System.Collections;
using System.Collections.Generic;
using DataGenies.Core.Behaviours;
using DataGenies.Core.Containers;
using DataGenies.Core.Models;
using DataGenies.Core.Publishers;
using DataGenies.Core.Receivers;

namespace DataGenies.Core.Services
{
    public interface IManagedService : IRestartable, IPublisherManagedService, IReceiverManagedService
    {
        int ApplicationInstanceEntityId { get; set; }
        IEnumerable<VirtualBinding> GetVirtualBindings();
        ServiceState State { get; set; }
        IContainer Container { get; }
        IEnumerable<BehaviourTemplate> BehaviourTemplates { get; }
        IEnumerable<WrapperBehaviourTemplate> WrapperBehaviours { get; }
    }
}