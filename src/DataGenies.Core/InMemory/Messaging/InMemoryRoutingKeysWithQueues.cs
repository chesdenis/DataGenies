using System.Collections.Concurrent;
using System.Collections.Generic;

namespace DataGenies.Core.InMemory.Messaging
{
    public class InMemoryRoutingKeysWithQueues : ConcurrentDictionary<string, List<InMemoryQueue>>
    {
        
    }
}