using System.Linq;
using DataGenies.AspNetCore.DataGeniesCore.Models.Contexts;

namespace DataGenies.AspNetCore.DataGeniesCore.Models.InMemory
{
    public class InMemorySchemaDataContext : ISchemaDataContext
    {
        public InMemoryDbSet<ApplicationTemplate> ApplicationTypes { get; }  
        
        public InMemoryDbSet<ApplicationInstance> ApplicationInstances { get; }  
        
        public InMemoryDbSet<Binding> Bindings { get; }

        public InMemorySchemaDataContext()
        {
            this.ApplicationTypes = new InMemoryDbSet<ApplicationTemplate>();
            this.ApplicationInstances = new InMemoryDbSet<ApplicationInstance>();
            this.Bindings = new InMemoryDbSet<Binding>();
        }

        IQueryable<ApplicationTemplate> ISchemaDataContext.ApplicationTypes => ApplicationTypes;

        IQueryable<ApplicationInstance> ISchemaDataContext.ApplicationInstances => ApplicationInstances;

        IQueryable<Binding> ISchemaDataContext.Bindings => Bindings;
    }
}