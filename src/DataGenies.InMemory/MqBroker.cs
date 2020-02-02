using System;

namespace DataGenies.InMemory
{
    public class MqBroker
    {
        public InMemoryExchanges Model { get; set; }

        public MqBroker()
        {
            this.Model = new InMemoryExchanges();
        }
    }
}