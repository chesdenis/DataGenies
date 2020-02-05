using System.Linq;
using DataGenies.Core.Models;

namespace DataGenies.InMemory
{
    public class SchemaDataContext : ISchemaDataContext
    {
        public DbSet<ApplicationTemplate> ApplicationTemplates { get; }
        public DbSet<ApplicationInstance> ApplicationInstances { get; }
        public DbSet<Binding> Bindings { get; }
        public DbSet<Behaviour> Behaviours { get; }
        public DbSet<Converter> Converters { get; }

        public SchemaDataContext()
        {
            this.ApplicationTemplates = new DbSet<ApplicationTemplate>();
            this.ApplicationInstances = new DbSet<ApplicationInstance>();
            this.Bindings = new DbSet<Binding>();
            this.Behaviours = new DbSet<Behaviour>();
            this.Converters = new DbSet<Converter>();
        }

        IQueryable<ApplicationTemplate> ISchemaDataContext.ApplicationTemplates => ApplicationTemplates;
        IQueryable<ApplicationInstance> ISchemaDataContext.ApplicationInstances => ApplicationInstances;
        IQueryable<Binding> ISchemaDataContext.Bindings => Bindings;
        IQueryable<Behaviour> ISchemaDataContext.Behaviours => Behaviours;
        IQueryable<Converter> ISchemaDataContext.Converters => Converters;
    }
}