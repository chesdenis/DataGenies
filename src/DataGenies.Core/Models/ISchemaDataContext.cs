using System.Linq;

namespace DataGenies.Core.Models
{
    public interface ISchemaDataContext
    {
        IQueryable<ApplicationTemplateEntity> ApplicationTemplates { get; }

        IQueryable<ApplicationInstanceEntity> ApplicationInstances { get; }

        IQueryable<BehaviourTemplateEntity> BehaviourTemplates { get; }

        IQueryable<BehaviourInstanceEntity> BehaviourInstances { get; }

        IQueryable<BindingEntity> Bindings { get; }
    }
}