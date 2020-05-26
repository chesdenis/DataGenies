namespace DG.Core.InMemoryMessageBroker
{
    public interface IReceiverBuilder
    {
        public IReceiver Build();
    }
}
