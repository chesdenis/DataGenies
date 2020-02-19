using System.Collections.Generic;
using DataGenies.Core.Behaviours;
using DataGenies.Core.Extensions;

namespace DataGenies.Core.Services
{
    public abstract class ManagedService : IManagedService
    {
        public IEnumerable<BehaviourTemplate> BehaviourTemplates { get; }
        public IEnumerable<WrapperBehaviourTemplate> WrapperBehaviours { get; }
        
        protected ManagedService( 
            IEnumerable<BehaviourTemplate> behaviourTemplates,
            IEnumerable<WrapperBehaviourTemplate> wrapperBehaviours)
        {
            BehaviourTemplates = behaviourTemplates;
            WrapperBehaviours = wrapperBehaviours;
            
            SetLinkToThisServiceInBehaviours();
        }
        
        private void SetLinkToThisServiceInBehaviours()
        {
            foreach (var behaviour in WrapperBehaviours)
            {
                behaviour.ManagedService = this;
            }

            foreach (var behaviour in BehaviourTemplates)
            {
                behaviour.ManagedService = this;
            }
        }

        public virtual void Start()
        {
            this.ManagedAction(OnStart, BehaviourScope.Service);
        }

        protected abstract void OnStart();

        public virtual void Stop()
        {
            this.ManagedAction(OnStop, BehaviourScope.Service);
        }

        protected abstract void OnStop();
    }
}