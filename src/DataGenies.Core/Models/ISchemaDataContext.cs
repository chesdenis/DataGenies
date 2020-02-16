using System.Linq;

namespace DataGenies.Core.Models
{
    public interface ISchemaDataContext
    {
        IQueryable<ApplicationTemplateEntity> ApplicationTemplates { get; }
        IQueryable<ApplicationInstanceEntity> ApplicationInstances { get; }
        IQueryable<BehaviourEntity> Behaviours { get; }
        IQueryable<BindingEntity> Bindings { get; }
    }
}