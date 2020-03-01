using System.Collections.Concurrent;
using DataGenies.Core.Models;

namespace DataGenies.Core.InMemory.Messaging
{
    public class InMemoryQueue : ConcurrentQueue<MqMessage>
    {
        public string Name { get; set; }
    }
}