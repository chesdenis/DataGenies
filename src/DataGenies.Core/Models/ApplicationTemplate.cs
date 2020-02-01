using System.Collections.Generic;

namespace DataGenies.Core.Models
{
    public class ApplicationTemplate
    {
        public ApplicationTemplate()
        {
            ApplicationInstance = new HashSet<ApplicationInstance>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }

        public string AssemblyPath { get; set; }
        
        public string ConfigTemplateJson { get; set; }

        public virtual ICollection<ApplicationInstance> ApplicationInstance { get; set; }

        public bool IsMatch(ApplicationTemplate template)
        {
            return this.Name == template.Name && this.Version == template.Version;
        }
    }
}