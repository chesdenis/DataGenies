namespace DG.Core.BrokerSynchronizer
{
    public interface IActualizerAction
    {
        bool CanExecute(BrokerCommand brokerCommand);

        void Execute(BrokerCommand brokerCommand);
    }
}
