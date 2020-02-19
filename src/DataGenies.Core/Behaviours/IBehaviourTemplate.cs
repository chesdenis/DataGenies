using System;
using DataGenies.Core.Containers;
using DataGenies.InMemory;

namespace DataGenies.Core.Behaviours
{
    public interface IBehaviourTemplate
    {
        void Execute();
    
        void Execute(IContainer container);

        void Execute(MqMessage message);

        void Execute(Exception exception);
        
        void Execute(IContainer container, Exception exception);
        
        void Execute(MqMessage message, Exception exception);
    }
}