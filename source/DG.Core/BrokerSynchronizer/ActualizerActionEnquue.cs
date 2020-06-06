namespace DG.Core.BrokerSynchronizer
{
    using DG.Core.InMemoryMessageBroker;

    public class ActualizerActionEnquue : IActualizerAction
    {
        private readonly IPublisher publisher;

        public bool CanExecute(BrokerCommand brokerCommand)
        {
            return brokerCommand.CommandType == BrokerCommandType.Enqueue;
        }

        public void Execute(BrokerCommand brokerCommand)
        {
            this.publisher.Publish(brokerCommand.Exchange, brokerCommand.Message);
        }
    }
}
