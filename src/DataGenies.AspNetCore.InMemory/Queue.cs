using System.Collections.Concurrent;

namespace DataGenies.AspNetCore.InMemory
{
    public class Queue : ConcurrentQueue<MqMessage>
    {
        public string Name { get; set; }
    }
}