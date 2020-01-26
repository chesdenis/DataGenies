using DataGenies.AspNetCore.DataGeniesCore.Abstraction.Publisher;
using DataGenies.AspNetCore.DataGeniesCore.Abstraction.Receiver;

namespace DataGenies.AspNetCore.DataGeniesCore.Abstraction
{
    public interface IStoppable
    {
        
    }
    
    public enum ConverterType
    {
        BeforePublish,
        AfterReceive
    }
    
    public interface IConverter
    {
        ConverterType Type { get; set; }

        byte[] Convert(byte[] data);
    }
    
    public interface IGuard : IReceiver, IPublisher, IRunnable
    {
        
    }
}