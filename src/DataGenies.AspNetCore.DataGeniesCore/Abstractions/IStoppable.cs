namespace DataGenies.AspNetCore.DataGeniesCore.Abstractions
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