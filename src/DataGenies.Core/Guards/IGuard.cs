using DataGenies.Core.Publishers;
using DataGenies.Core.Receivers;
using DataGenies.Core.Services;

namespace DataGenies.Core.Guards
{
    public interface IGuard : IReceiver, IPublisher, IStartable
    {
    }
}