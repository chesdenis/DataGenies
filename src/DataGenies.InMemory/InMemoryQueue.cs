using System.Collections.Concurrent;

namespace DataGenies.InMemory
{
    public class InMemoryQueue : ConcurrentQueue<MqMessage>
    {
        public string Name { get; set; }
    }
}