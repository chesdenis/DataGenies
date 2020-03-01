namespace DataGenies.Core.InMemory.Messaging
{
    public class InMemoryMqBroker
    {
        public InMemoryExchanges Model { get; set; }

        public InMemoryMqBroker()
        {
            this.Model = new InMemoryExchanges();
        }
    }
}