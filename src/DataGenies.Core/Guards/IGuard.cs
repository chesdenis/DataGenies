using DataGenies.Core.Publishers;
using DataGenies.Core.Receivers;
using DataGenies.Core.Roles;

namespace DataGenies.Core.Guards
{
    public interface IGuard : IReceiver, IPublisher, IStartable
    {
        
    }
}