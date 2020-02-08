using System.Collections.Generic;

namespace DataGenies.Core.Models
{
    public partial class ApplicationInstanceEntity
    {
        public ApplicationInstanceEntity()
        {
            IncomingBindings = new HashSet<BindingEntity>();
            OutcomingBindings = new HashSet<BindingEntity>();
        }

        public int Id { get; set; }
        public int TemplateId { get; set; }
        public string Name { get; set;  }
        public string ConfigJson { get; set;  }

        public virtual ApplicationTemplateEntity TemplateEntity { get; set;  }

        public virtual ICollection<BehaviourEntity> Behaviours { get; set; }

        public virtual ICollection<ConverterEntity> Converters { get; set; }

        public virtual ICollection<BindingEntity> IncomingBindings { get; set; }
        
        public virtual ICollection<BindingEntity> OutcomingBindings { get; set; }
    }
}