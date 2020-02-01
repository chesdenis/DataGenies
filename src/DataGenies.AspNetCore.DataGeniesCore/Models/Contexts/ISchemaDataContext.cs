using System.Linq;

namespace DataGenies.AspNetCore.DataGeniesCore.Models.Contexts
{
    public interface ISchemaDataContext
    {
        IQueryable<ApplicationTemplate> ApplicationTypes { get; }
        IQueryable<ApplicationInstance> ApplicationInstances { get; }
        IQueryable<Binding> Bindings { get; }
    }
}