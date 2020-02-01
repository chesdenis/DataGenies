using System.Linq;

namespace DataGenies.Core.Models
{
    public partial class Binding
    {
        public int IncomingApplicationInstanceId { get; set; }
        public int OutcomingApplicationInstanceId { get; set; }

        public virtual ApplicationInstance IncomingBinding { get; set; }
        public virtual ApplicationInstance OutcomingBinding { get; set; }
    }
 
}