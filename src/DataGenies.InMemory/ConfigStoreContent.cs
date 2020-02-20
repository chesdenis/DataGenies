using DataGenies.Core.Models;

namespace DataGenies.InMemory
{
    public class ConfigStoreContent
    {
        public ApplicationTemplateEntity[] ApplicationTemplates { get; set; }
        public ApplicationInstanceEntity[] ApplicationInstances { get; set; }
        public BehaviourTemplateEntity[] BehavioursTemplates { get; set; }
        public BehaviourInstanceEntity[] BehavioursInstances { get; set; }
        public BindingEntity[] Bindings { get; set; }
        public object ApplicationInstancesBehaviours { get; set; }
    }
}