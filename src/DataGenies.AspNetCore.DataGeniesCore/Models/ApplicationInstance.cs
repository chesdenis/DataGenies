using System.Collections.Generic;

namespace DataGenies.AspNetCore.DataGeniesCore.Models
{
    public partial class ApplicationInstance
    {
        public ApplicationInstance()
        {
            IncomingBindings = new HashSet<Binding>();
            OutcomingBindings = new HashSet<Binding>();
        }

        public int InstanceId { get; set; }
        public int TypeId { get; set; }
        public string InstanceName { get; set;  }
        public string ConfigJson { get; set;  }

        public virtual ApplicationTemplate Template { get; set;  }
        public virtual ICollection<Binding> IncomingBindings { get; set; }
        
        public virtual ICollection<Binding> OutcomingBindings { get; set; }
    }
}