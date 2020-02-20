using System.Collections.Concurrent;

namespace DataGenies.Core.Tests.Integration.Stubs.Mq
{
    public class InMemoryExchanges : ConcurrentDictionary<string, InMemoryRoutingKeysWithQueues>
    {
        
    }
}