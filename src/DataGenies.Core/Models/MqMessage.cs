namespace DataGenies.InMemory
{
    public class MqMessage
    {
        public byte[] Body { get; set; }

        public string RoutingKey { get; set; } = "#";
    }
}