using System.Collections.Generic;
using System.Collections.Concurrent;
namespace DataGenies.AspNetCore.DataGeniesCore.Models.InMemory
{
    public class InMemoryQueue : ConcurrentQueue<InMemoryMqMessage>
    {
        public string Name { get; set; }
    }
}