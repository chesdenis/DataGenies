namespace DG.Core.InMemoryMessageBroker
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;

    public class InMemoryRoutingKeysWithQueues : ConcurrentDictionary<string, List<InMemoryQueue>>
    {

    }
}
