using System.Collections.Generic;

namespace DataGenies.Core.Models
{
    public class ApplicationTemplate
    {
        public ApplicationTemplate()
        {
            ApplicationInstance = new HashSet<ApplicationInstance>();
        }

        public int TypeId { get; set; }
        public string TypeName { get; set; }
        public string TypeVersion { get; set; }

        public string AssemblyPath { get; set; }
        
        public string ConfigTemplateJson { get; set; }

        public virtual ICollection<ApplicationInstance> ApplicationInstance { get; set; }

        public bool IsMatch(ApplicationTemplate template)
        {
            return this.TypeName == template.TypeName && this.TypeVersion == template.TypeVersion;
        }
    }
}