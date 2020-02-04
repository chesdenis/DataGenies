using System.Linq;

namespace DataGenies.Core.Models
{
    public partial class Binding
    {
        public int ReceiverId { get; set; }
        public int PublisherId { get; set; }

        public string ReceiverRoutingKey { get; set; }

        public virtual ApplicationInstance ReceiverApplicationInstance { get; set; }
        public virtual ApplicationInstance PublisherApplicationInstance { get; set; }
    }
}