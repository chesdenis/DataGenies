namespace DG.Core.BrokerSynchronizer
{
    public class ActualizerActionDequeue : IActualizerAction
    {
        public bool CanExecute(BrokerCommand brokerCommand)
        {
            return brokerCommand.CommandType == BrokerCommandType.Dequeue;
        }

        public void Execute(BrokerCommand brokerCommand)
        {
            // need to get rid of message in certain queue
            throw new System.NotImplementedException();
        }
    }
}
