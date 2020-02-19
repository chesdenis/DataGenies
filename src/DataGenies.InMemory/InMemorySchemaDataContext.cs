using System.Linq;
using DataGenies.Core.Models;

namespace DataGenies.InMemory
{
    public class InMemorySchemaDataContext : ISchemaDataContext
    {
        public InMemoryDbSet<ApplicationTemplateEntity> ApplicationTemplates { get; }
        public InMemoryDbSet<ApplicationInstanceEntity> ApplicationInstances { get; }
        public InMemoryDbSet<BindingEntity> Bindings { get; }
        public InMemoryDbSet<BehaviourTemplateEntity> BehaviourTemplates { get; }
        public InMemoryDbSet<BehaviourInstanceEntity> BehaviourInstances { get; }
        
        public InMemorySchemaDataContext()
        {
            this.ApplicationTemplates = new InMemoryDbSet<ApplicationTemplateEntity>();
            this.ApplicationInstances = new InMemoryDbSet<ApplicationInstanceEntity>();
            this.Bindings = new InMemoryDbSet<BindingEntity>();
            this.BehaviourTemplates = new InMemoryDbSet<BehaviourTemplateEntity>();
            this.BehaviourInstances = new InMemoryDbSet<BehaviourInstanceEntity>();
        }

        IQueryable<ApplicationTemplateEntity> ISchemaDataContext.ApplicationTemplates => ApplicationTemplates;
        IQueryable<ApplicationInstanceEntity> ISchemaDataContext.ApplicationInstances => ApplicationInstances;
        IQueryable<BindingEntity> ISchemaDataContext.Bindings => Bindings;
        IQueryable<BehaviourTemplateEntity> ISchemaDataContext.BehaviourTemplates => BehaviourTemplates;
        IQueryable<BehaviourInstanceEntity> ISchemaDataContext.BehaviourInstances => BehaviourInstances;
    }
}