using System;

namespace DataGenies.InMemory
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