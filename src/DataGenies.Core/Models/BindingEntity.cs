using System.Linq;

namespace DataGenies.Core.Models
{
    public class CustomPublishParameter
    {
        public string Exchange { get; set; }
        public string RoutingKey { get; set; }
    }

    public class CustomListenParameter
    {
        public string QueueName { get; set; }
    }

    public partial class BindingEntity
    {
        public int ReceiverId { get; set; }
        public int PublisherId { get; set; }

        public string ReceiverRoutingKey { get; set; }

        public virtual ApplicationInstanceEntity ReceiverApplicationInstanceEntity { get; set; }
        public virtual ApplicationInstanceEntity PublisherApplicationInstanceEntity { get; set; }
    }
}