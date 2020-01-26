using DataGenies.AspNetCore.DataGeniesCore.Abstractions.Publishers;
using DataGenies.AspNetCore.DataGeniesCore.Abstractions.Receivers;

namespace DataGenies.AspNetCore.DataGeniesCore.Abstractions
{
    public interface IGuard : IReceiver, IPublisher, IStartable
    {
        
    }
}