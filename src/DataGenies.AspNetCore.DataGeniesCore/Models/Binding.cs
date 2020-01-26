namespace DataGenies.AspNetCore.DataGeniesCore.Models
{
    public partial class Binding
    {
        public int PublisherInstanceId { get; set; }
        public int ReceiverInstanceId { get; set; }

        public virtual ApplicationInstance ReceiverInstance { get; set; }
        public virtual ApplicationInstance PublisherInstance { get; set; }
    }
}