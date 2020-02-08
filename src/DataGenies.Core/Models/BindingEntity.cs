using System.Linq;

namespace DataGenies.Core.Models
{
    public partial class BindingEntity
    {
        public int ReceiverId { get; set; }
        public int PublisherId { get; set; }

        public string ReceiverRoutingKey { get; set; }

        public virtual ApplicationInstanceEntity ReceiverApplicationInstanceEntity { get; set; }
        public virtual ApplicationInstanceEntity PublisherApplicationInstanceEntity { get; set; }
    }
}