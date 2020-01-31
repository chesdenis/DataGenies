using System.Linq;

namespace DataGenies.AspNetCore.DataGeniesCore.Models.Contexts
{
    public interface IDataGeniesDataContext
    {
        IQueryable<ApplicationType> ApplicationTypes { get; }
        IQueryable<ApplicationInstance> ApplicationInstances { get; }
        IQueryable<Binding> Bindings { get; }
    }
}