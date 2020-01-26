using System.Collections.Generic;

namespace DataGenies.AspNetCore.DataGeniesCore.Models
{
    public partial class ApplicationInstance
    {
        public ApplicationInstance()
        {
            Receivers = new HashSet<Binding>();
            Publishers = new HashSet<Binding>();
        }

        public int InstanceId { get; set; }
        public int TypeId { get; set; }
        public string InstanceName { get; set;  }
        public string ConfigJson { get; set;  }

        public virtual ApplicationType Type { get; set;  }
        public virtual ICollection<Binding> Receivers { get; set; }
        
        public virtual ICollection<Binding> Publishers { get; set; }
    }
}