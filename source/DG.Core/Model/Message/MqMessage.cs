namespace DG.Core.Model.Message
{
    public class MqMessage
    {
        public byte[] Body { get; set; }

        public string RoutingKey { get; set; } = "#";
    }
}
