using System.Collections.Concurrent;

namespace DataGenies.InMemory
{
    public class Queue : ConcurrentQueue<MqMessage>
    {
        public string Name { get; set; }
    }
}