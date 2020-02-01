using DataGenies.AspNetCore.DataGeniesCore.Publishers;
using DataGenies.AspNetCore.DataGeniesCore.Receivers;
using DataGenies.AspNetCore.DataGeniesCore.Roles;

namespace DataGenies.AspNetCore.DataGeniesCore.Guards
{
    public interface IGuard : IReceiver, IPublisher, IStartable
    {
        
    }
}