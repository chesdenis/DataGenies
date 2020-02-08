using System.Collections.Concurrent;
using System.Collections.Generic;

namespace DataGenies.InMemory
{
    public class InMemoryRoutingKeysWithQueues : ConcurrentDictionary<string, List<InMemoryQueue>>
    {
        
    }
}