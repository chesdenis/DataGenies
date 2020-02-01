using System.Linq;
using DataGenies.AspNetCore.DataGeniesCore.Models;
using DataGenies.AspNetCore.DataGeniesCore.Models.Contexts;

namespace DataGenies.AspNetCore.InMemory
{
    public class SchemaDataContext : ISchemaDataContext
    {
        public DbSet<ApplicationTemplate> ApplicationTypes { get; }  
        
        public DbSet<ApplicationInstance> ApplicationInstances { get; }  
        
        public DbSet<Binding> Bindings { get; }

        public SchemaDataContext()
        {
            this.ApplicationTypes = new DbSet<ApplicationTemplate>();
            this.ApplicationInstances = new DbSet<ApplicationInstance>();
            this.Bindings = new DbSet<Binding>();
        }

        IQueryable<ApplicationTemplate> ISchemaDataContext.ApplicationTypes => ApplicationTypes;

        IQueryable<ApplicationInstance> ISchemaDataContext.ApplicationInstances => ApplicationInstances;

        IQueryable<Binding> ISchemaDataContext.Bindings => Bindings;
    }
}