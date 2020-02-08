﻿using System.Collections.Generic;

namespace DataGenies.Core.Models
{
    public class BehaviourEntity
    {
        public BehaviourEntity()
        {
            ApplicationInstances = new HashSet<ApplicationInstanceEntity>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }

        public string AssemblyPath { get; set; }
        
        public virtual ICollection<ApplicationInstanceEntity> ApplicationInstances { get; set; }

        public bool IsMatch(BehaviourEntity behaviourEntity)
        {
            return this.Name == behaviourEntity.Name && this.Version == behaviourEntity.Version;
        }
    }
}