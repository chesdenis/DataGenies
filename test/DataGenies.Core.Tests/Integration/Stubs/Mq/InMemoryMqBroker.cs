namespace DataGenies.Core.Tests.Integration.Stubs.Mq
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