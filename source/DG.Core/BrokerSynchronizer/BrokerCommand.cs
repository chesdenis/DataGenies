using DG.Core.Model.Message;

namespace DG.Core.BrokerSynchronizer
{
    public enum BrokerCommandType
    {
        Enqueue,
        Dequeue,
    }

    public class BrokerCommand
    {
        public BrokerCommandType CommandType { get; }

        public string Exchange { get; }

        public MqMessage Message { get; }
    }
}
