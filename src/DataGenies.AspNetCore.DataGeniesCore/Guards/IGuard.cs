using DataGenies.AspNetCore.DataGeniesCore.ApplicationTemplates;
using DataGenies.AspNetCore.DataGeniesCore.Publishers;
using DataGenies.AspNetCore.DataGeniesCore.Receivers;

namespace DataGenies.AspNetCore.DataGeniesCore.Guards
{
    public interface IGuard : IReceiver, IPublisher, IStartable
    {
        
    }
}