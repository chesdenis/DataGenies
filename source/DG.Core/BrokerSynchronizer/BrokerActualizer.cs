namespace DG.Core.BrokerSynchronizer
{
    using System.Collections.Generic;
    using System.Linq;

    public class BrokerActualizer
    {
        private readonly IEnumerable<IActualizerAction> actualizerActions;

        public void ExecuteCommands(BrokerCommand[] brokerCommands)
        {
            foreach (var brokerCommand in brokerCommands)
            {
                this.actualizerActions
                    .FirstOrDefault(aa => aa.CanExecute(brokerCommand))?.Execute(brokerCommand);
            }
        }
    }
}
