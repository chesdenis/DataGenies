using System.Collections.Generic;
using System.Threading;

namespace DataGenies.AspNetCore.DataGeniesCore.Models.InMemory
{
    public class InMemoryMqBroker
    {
        public Dictionary<string, InMemoryQueue> ExchangesAndBoundQueues { get; set; }
    }
}