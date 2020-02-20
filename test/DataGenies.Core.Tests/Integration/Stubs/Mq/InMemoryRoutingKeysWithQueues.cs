using System.Collections.Concurrent;
using System.Collections.Generic;

namespace DataGenies.Core.Tests.Integration.Stubs.Mq
{
    public class InMemoryRoutingKeysWithQueues : ConcurrentDictionary<string, List<InMemoryQueue>>
    {
        
    }
}