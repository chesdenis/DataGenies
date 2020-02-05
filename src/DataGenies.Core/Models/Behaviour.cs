using System.Collections.Generic;

namespace DataGenies.Core.Models
{
    public class Behaviour
    {
        public Behaviour()
        {
            ApplicationInstances = new HashSet<ApplicationInstance>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }

        public string AssemblyPath { get; set; }
        
        public virtual ICollection<ApplicationInstance> ApplicationInstances { get; set; }

        public bool IsMatch(Behaviour behaviour)
        {
            return this.Name == behaviour.Name && this.Version == behaviour.Version;
        }
    }
}