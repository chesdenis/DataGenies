using System.Collections.Concurrent;

namespace DataGenies.Core.InMemory.Messaging
{
    public class InMemoryExchanges : ConcurrentDictionary<string, InMemoryRoutingKeysWithQueues>
    {
        
    }
}