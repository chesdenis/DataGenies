using DG.Core.Model.Enums;

namespace DG.Core.Model.Output
{
    public class StateReport
    {
        public Status Status { get; set; }

        public string Message { get; set; }

        public string NodeName { get; set; }

        public int ReplicaId { get; set; }
    }
}