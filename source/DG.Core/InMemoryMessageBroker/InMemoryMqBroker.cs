namespace DG.Core.InMemoryMessageBroker
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
