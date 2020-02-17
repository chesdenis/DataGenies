using DataGenies.Core.Containers;
using DataGenies.InMemory;

namespace DataGenies.Core.Behaviours
{
    public interface IBasicBehaviour : IBehaviour
    {
        void Execute();
    
        void Execute(IContainer container);

        void Execute(MqMessage message);
    }
}