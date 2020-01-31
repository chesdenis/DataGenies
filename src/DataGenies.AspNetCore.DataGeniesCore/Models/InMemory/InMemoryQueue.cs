using System.Collections.Generic;

namespace DataGenies.AspNetCore.DataGeniesCore.Models.InMemory
{
    public class InMemoryQueue : Queue<InMemoryMqMessage>
    {
        public string Name { get; set; }
    }
}