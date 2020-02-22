using System.Collections.Generic;
using DataGenies.Core.Behaviours;
using DataGenies.Core.Containers;
using DataGenies.Core.Models;
using DataGenies.Core.Publishers;
using DataGenies.Core.Receivers;

namespace DataGenies.Core.Services
{
    public interface IManagedService : IRestartable, IPublisher, IReceiver
    {
        int ApplicationInstanceEntityId { get; set; }
        List<CustomPublishParameter> CustomPublishParameters { get; set; }
        List<CustomListenParameter> CustomListenParameters { get; set; }
        
        ServiceState State { get; set; }
        IContainer Container { get; }
        IEnumerable<BehaviourTemplate> BehaviourTemplates { get; }
        IEnumerable<WrapperBehaviourTemplate> WrapperBehaviours { get; }
    }
}