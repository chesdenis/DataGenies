using System.Collections.Generic;

namespace DataGenies.Core.Models
{
    public class Converter
    {
        public Converter()
        {
            ApplicationInstances = new HashSet<ApplicationInstance>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }

        public string AssemblyPath { get; set; }
        
        public virtual ICollection<ApplicationInstance> ApplicationInstances { get; set; }

        public bool IsMatch(Converter converter)
        {
            return this.Name == converter.Name && this.Version == converter.Version;
        }
    }
}