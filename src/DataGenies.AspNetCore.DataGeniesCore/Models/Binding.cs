using System.Linq;

namespace DataGenies.AspNetCore.DataGeniesCore.Models
{
    public partial class Binding
    {
        public int IncomingApplicationInstanceId { get; set; }
        public int OutcomingApplicationInstanceId { get; set; }

        public virtual ApplicationInstance IncomingBinding { get; set; }
        public virtual ApplicationInstance OutcomingBinding { get; set; }
    }
 
}