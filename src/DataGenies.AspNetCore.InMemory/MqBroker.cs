using System;
using System.Collections.Generic;

namespace DataGenies.AspNetCore.InMemory
{
    public class MqBroker
    {
        public IEnumerable<Tuple<string, Queue>> ExchangesAndBoundQueues { get; set; }
    }
}