using System;
using DataGenies.Core.Containers;

namespace DataGenies.Core.Behaviours
{
    public interface IBehaviourOnException : IBehaviour
    {
        void Execute(Exception exception);

        void Execute(IContainer arg, Exception exception);
    }
}