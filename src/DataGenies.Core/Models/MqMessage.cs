namespace DataGenies.Core.Models
{
    public class MqMessage
    {
        public byte[] Body { get; set; }

        public string RoutingKey { get; set; } = "#";
    }
}