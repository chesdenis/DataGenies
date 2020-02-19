using System;
using DataGenies.Core.Containers;
using DataGenies.InMemory;

namespace DataGenies.Core.Behaviours
{
    public abstract class BehaviourTemplate : IBehaviourTemplate
    {
        public string Name { get; set; }
        public string Description { get; set; }
        
        public BehaviourScope BehaviourScope { get; set; }
        
        public BehaviourType BehaviourType { get; set; }

        public virtual void Execute()
        {
            throw new NotImplementedException();
        }

        public virtual void Execute(IContainer container)
        {
            throw new NotImplementedException();
        }

        public virtual void Execute(MqMessage message)
        {
            throw new NotImplementedException();
        }

        public virtual void Execute(Exception exception)
        {
            throw new NotImplementedException();
        }

        public virtual void Execute(IContainer container, Exception exception)
        {
            throw new NotImplementedException();
        }

        public virtual void Execute(MqMessage message, Exception exception)
        {
            throw new NotImplementedException();
        }
    }
}