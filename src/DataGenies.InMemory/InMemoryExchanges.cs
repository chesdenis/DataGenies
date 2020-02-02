using System.Collections.Concurrent;
using System.Collections.Generic;

namespace DataGenies.InMemory
{
    public class InMemoryExchanges : ConcurrentDictionary<string, InMemoryRoutingKeysWithQueues>
    {
        
    }
}