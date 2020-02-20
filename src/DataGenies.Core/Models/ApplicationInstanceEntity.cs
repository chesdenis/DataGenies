using System.Collections.Generic;

namespace DataGenies.Core.Models
{
    public class ApplicationInstanceEntity
    {
        public ApplicationInstanceEntity()
        {
            IncomingBindings = new HashSet<BindingEntity>();
            OutcomingBindings = new HashSet<BindingEntity>();
        }

        public int Id { get; set; }

        public int TemplateId { get; set; }

        public int InstanceCount { get; set; }

        public string Name { get; set; }

        public string ParametersDictAsJson { get; set; }

        public virtual ApplicationTemplateEntity TemplateEntity { get; set; }

        public virtual ICollection<BehaviourInstanceEntity> Behaviours { get; set; }

        public virtual ICollection<BindingEntity> IncomingBindings { get; set; }

        public virtual ICollection<BindingEntity> OutcomingBindings { get; set; }
    }
}