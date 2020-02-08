using System.Linq;
using DataGenies.Core.Models;

namespace DataGenies.InMemory
{
    public class SchemaDataContext : ISchemaDataContext
    {
        public DbSet<ApplicationTemplateEntity> ApplicationTemplates { get; }
        public DbSet<ApplicationInstanceEntity> ApplicationInstances { get; }
        public DbSet<BindingEntity> Bindings { get; }
        public DbSet<BehaviourEntity> Behaviours { get; }
        public DbSet<ConverterEntity> Converters { get; }

        public SchemaDataContext()
        {
            this.ApplicationTemplates = new DbSet<ApplicationTemplateEntity>();
            this.ApplicationInstances = new DbSet<ApplicationInstanceEntity>();
            this.Bindings = new DbSet<BindingEntity>();
            this.Behaviours = new DbSet<BehaviourEntity>();
            this.Converters = new DbSet<ConverterEntity>();
        }

        IQueryable<ApplicationTemplateEntity> ISchemaDataContext.ApplicationTemplates => ApplicationTemplates;
        IQueryable<ApplicationInstanceEntity> ISchemaDataContext.ApplicationInstances => ApplicationInstances;
        IQueryable<BindingEntity> ISchemaDataContext.Bindings => Bindings;
        IQueryable<BehaviourEntity> ISchemaDataContext.Behaviours => Behaviours;
        IQueryable<ConverterEntity> ISchemaDataContext.Converters => Converters;
    }
}