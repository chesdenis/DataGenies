using System.Collections.Generic;
using DataGenies.Core.Behaviours;

namespace DataGenies.Core.Models
{
    public class BehaviourInstanceEntity
    {
        public BehaviourInstanceEntity()
        {
            ApplicationInstances = new HashSet<ApplicationInstanceEntity>();
        }

        public int Id { get; set; }
        
        public int TemplateId { get; set; }
        
        public string Name { get; set; }
        
        public string ParametersDictAsJson { get; set; }
        
        public BehaviourType BehaviourType { get; set; }

        public BehaviourScope BehaviourScope { get; set; }

        public virtual BehaviourTemplateEntity TemplateEntity { get; set; }
        
        public virtual ICollection<ApplicationInstanceEntity> ApplicationInstances { get; set; }

        public bool IsMatch(BehaviourInstanceEntity behaviourEntity)
        {
            return this.Name == behaviourEntity.Name && this.Id == behaviourEntity.Id;
        }
    }
}