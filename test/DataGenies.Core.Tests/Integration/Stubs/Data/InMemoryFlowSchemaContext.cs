using System.Linq;
using DataGenies.Core.Models;

namespace DataGenies.Core.Tests.Integration.Stubs.Data
{
    public class InMemoryFlowSchemaContext : IFlowSchemaContext
    {
        public InMemoryDbSet<ApplicationTemplateEntity> ApplicationTemplates { get; }
        public InMemoryDbSet<ApplicationInstanceEntity> ApplicationInstances { get; }
        public InMemoryDbSet<BindingEntity> Bindings { get; }
        public InMemoryDbSet<BehaviourTemplateEntity> BehaviourTemplates { get; }
        public InMemoryDbSet<BehaviourInstanceEntity> BehaviourInstances { get; }
        
        public InMemoryFlowSchemaContext()
        {
            this.ApplicationTemplates = new InMemoryDbSet<ApplicationTemplateEntity>();
            this.ApplicationInstances = new InMemoryDbSet<ApplicationInstanceEntity>();
            this.Bindings = new InMemoryDbSet<BindingEntity>();
            this.BehaviourTemplates = new InMemoryDbSet<BehaviourTemplateEntity>();
            this.BehaviourInstances = new InMemoryDbSet<BehaviourInstanceEntity>();
        }

        IQueryable<ApplicationTemplateEntity> IFlowSchemaContext.ApplicationTemplates => ApplicationTemplates;
        IQueryable<ApplicationInstanceEntity> IFlowSchemaContext.ApplicationInstances => ApplicationInstances;
        IQueryable<BindingEntity> IFlowSchemaContext.Bindings => Bindings;
        IQueryable<BehaviourTemplateEntity> IFlowSchemaContext.BehaviourTemplates => BehaviourTemplates;
        IQueryable<BehaviourInstanceEntity> IFlowSchemaContext.BehaviourInstances => BehaviourInstances;
    }
}