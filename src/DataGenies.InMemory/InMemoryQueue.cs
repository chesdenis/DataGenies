using System.Collections.Concurrent;
using DataGenies.Core.Models;

namespace DataGenies.InMemory
{
    public class InMemoryQueue : ConcurrentQueue<MqMessage>
    {
        public string Name { get; set; }
    }
}