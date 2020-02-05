using System.Linq;

namespace DataGenies.Core.Models
{
    public interface ISchemaDataContext
    {
        IQueryable<ApplicationTemplate> ApplicationTemplates { get; }
        IQueryable<ApplicationInstance> ApplicationInstances { get; }
        IQueryable<Behaviour> Behaviours { get; }
        IQueryable<Converter> Converters { get; }
        IQueryable<Binding> Bindings { get; }
    }
}