using System.Collections.Generic;

namespace DataGenies.Core.Models
{
    public class ConverterEntity
    {
        public ConverterEntity()
        {
            ApplicationInstances = new HashSet<ApplicationInstanceEntity>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }

        public string AssemblyPath { get; set; }
        
        public virtual ICollection<ApplicationInstanceEntity> ApplicationInstances { get; set; }

        public bool IsMatch(ConverterEntity converterEntity)
        {
            return this.Name == converterEntity.Name && this.Version == converterEntity.Version;
        }
    }
}