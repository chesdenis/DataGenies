using System.Linq;

namespace DataGenies.Core.Models
{
    public interface ISchemaDataContext
    {
        IQueryable<ApplicationTemplate> ApplicationTemplates { get; }
        IQueryable<ApplicationInstance> ApplicationInstances { get; }
        IQueryable<Binding> Bindings { get; }
    }
}