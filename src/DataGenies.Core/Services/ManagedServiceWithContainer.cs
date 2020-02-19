using System.Collections.Generic;
using DataGenies.Core.Behaviours;
using DataGenies.Core.Containers;
using DataGenies.Core.Extensions;

namespace DataGenies.Core.Services
{
    public abstract class ManagedServiceWithContainer : ManagedService, IManagedServiceWithContainer
    {
        public IContainer Container { get; }
        
        protected ManagedServiceWithContainer(
            IContainer container,
            IEnumerable<BehaviourTemplate> behaviourTemplates,
            IEnumerable<WrapperBehaviourTemplate> wrapperBehaviours) : base(behaviourTemplates, wrapperBehaviours)
        {
            Container = container;
        }
        
        public override void Start()
        {
            this.ManagedActionWithContainer((x) => OnStart(), Container, BehaviourScope.Service);
        }
        
        public override void Stop()
        {
            this.ManagedActionWithContainer((x)=>OnStop(), Container, BehaviourScope.Service);
        }
    }
}