using DataGenies.Core.Containers;

namespace DataGenies.Core.Services
{
    public interface IManagedServiceWithContainer : IManagedService
    {
        IContainer Container { get; }
    }
}