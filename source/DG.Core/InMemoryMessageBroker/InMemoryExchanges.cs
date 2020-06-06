namespace DG.Core.InMemoryMessageBroker
{
    using System.Collections.Concurrent;

    public class InMemoryExchanges : ConcurrentDictionary<string, InMemoryRoutingKeysWithQueues>
    {
    }
}
