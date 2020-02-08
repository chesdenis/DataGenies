using System.Collections.Generic;

namespace DataGenies.Core.Models
{
    public class ApplicationTemplateEntity
    {
        public ApplicationTemplateEntity()
        {
            ApplicationInstance = new HashSet<ApplicationInstanceEntity>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }

        public string AssemblyPath { get; set; }
        
        public string ConfigTemplateJson { get; set; }

        public virtual ICollection<ApplicationInstanceEntity> ApplicationInstance { get; set; }

        public bool IsMatch(ApplicationTemplateEntity templateEntity)
        {
            return this.Name == templateEntity.Name && this.Version == templateEntity.Version;
        }
    }
}