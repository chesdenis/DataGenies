using System.Collections.Generic;

namespace DataGenies.AspNetCore.DataGeniesCore.Models
{
    public class ApplicationType
    {
        public ApplicationType()
        {
            ApplicationInstance = new HashSet<ApplicationInstance>();
        }

        public int TypeId { get; set; }
        public string TypeName { get; set; }
        public string TypeVersion { get; set; }

        public string AssemblyPath { get; set; }
        
        public string ConfigTemplateJson { get; set; }

        public virtual ICollection<ApplicationInstance> ApplicationInstance { get; set; }

    }
}