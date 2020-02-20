using System.Collections.Concurrent;
using DataGenies.Core.Models;

namespace DataGenies.Core.Tests.Integration.Stubs.Mq
{
    public class InMemoryQueue : ConcurrentQueue<MqMessage>
    {
        public string Name { get; set; }
    }
}