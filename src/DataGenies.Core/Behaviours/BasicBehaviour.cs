using System;
using DataGenies.Core.Containers;
using DataGenies.InMemory;

namespace DataGenies.Core.Behaviours
{
    public abstract class BasicBehaviour : IBasicBehaviour
    {
        public string Name { get; set; }
        public string Description { get; set; }
        
        public abstract BehaviourScope BehaviourScope { get; set; }
        
        public abstract BehaviourType BehaviourType { get; set; }

        public virtual void Execute()
        {
            throw new NotImplementedException();
        }

        public virtual void Execute(IContainer arg)
        {
            throw new NotImplementedException();
        }

        public virtual void Execute(MqMessage message)
        {
            throw new NotImplementedException();
        }
    }
}