using System.Collections.Generic;

namespace DataGenies.Core.Models
{
    public class BehaviourTemplateEntity
    {
        public BehaviourTemplateEntity()
        {
            this.BehaviourInstances = new HashSet<BehaviourInstanceEntity>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Version { get; set; }

        public string AssemblyPath { get; set; }

        public string ConfigTemplateJson { get; set; }

        public virtual ICollection<BehaviourInstanceEntity> BehaviourInstances { get; set; }

        public bool IsMatch(BehaviourTemplateEntity behaviourEntity)
        {
            return this.Name == behaviourEntity.Name && this.Version == behaviourEntity.Version;
        }
    }
}