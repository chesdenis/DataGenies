namespace DataGenies.AspNetCore.DataGeniesCore.Models.InMemory
{
    public class InMemoryMqMessage
    {
        public byte[] Body { get; set; }

        public string RoutingKey { get; set; }
    }
}