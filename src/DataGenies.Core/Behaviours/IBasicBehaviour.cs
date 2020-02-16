using DataGenies.Core.Containers;

namespace DataGenies.Core.Behaviours
{
    public interface IBasicBehaviour : IBehaviour
    {
        void Execute();
    
        void Execute(IContainer arg);
    }
}