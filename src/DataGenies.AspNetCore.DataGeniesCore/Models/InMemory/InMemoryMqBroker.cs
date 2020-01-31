using System;
using System.Collections.Generic;
using System.Threading;

namespace DataGenies.AspNetCore.DataGeniesCore.Models.InMemory
{
    public class InMemoryMqBroker
    {
        public IEnumerable<Tuple<string, InMemoryQueue>> ExchangesAndBoundQueues { get; set; }
    }
}