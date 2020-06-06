namespace DG.Core.BrokerSynchronizer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DG.Core.Extensions;

    public class BrokerUpdateManager
    {
        private readonly BrokerActualizer brokerАnnunciator;
        private readonly Dictionary<string, DateTime> brokerUpdatesStamps;
        private readonly UpdatesQueue brokerUpdatesQueue;
        private readonly BrokerUpdateProvider brokerUpdateProvider;

        public void ImplementUpdate(BrokerUpdate brokerUpdate)
        {
            if (!this.brokerUpdatesStamps.ContainsKey(brokerUpdate.Hash))
            {
                foreach (var updateHash in brokerUpdate.PreviousUpdates)
                {
                    if (!this.brokerUpdatesQueue.Any(buq => buq.Hash == updateHash))
                    {
                        var missedUpdate = this.brokerUpdateProvider.GetUpdate(updateHash);

                        if (missedUpdate == null)
                        {
                            throw new Exception("Unable to sync message broker system.");
                        }

                        this.brokerUpdatesQueue.EnqueueUpdate(missedUpdate);
                        this.ImplementUpdate(missedUpdate);
                    }
                }

                this.brokerАnnunciator.ExecuteCommands(brokerUpdate.BrokerCommands);
                this.brokerUpdatesStamps.Add(brokerUpdate.Hash, DateTime.Now);
            }
        }

        public BrokerUpdate CreateUpdate(BrokerCommand[] brokerCommands)
        {
            var previousUpdates = new List<string>();

            foreach (var update in this.brokerUpdatesQueue)
            {
                previousUpdates.Add(update.Hash);
            }

            var brokerUpdate = new BrokerUpdate()
            {
                Created = DateTime.Now,
                PreviousUpdates = previousUpdates,
                BrokerCommands = brokerCommands,
            };
            brokerUpdate.Hash = brokerUpdate.CalculateMd5Hash();

            return brokerUpdate;
        }

        public BrokerUpdate GetUpdate(string updateHash)
        {
            return this.brokerUpdatesQueue.FirstOrDefault(buq => buq.Hash == updateHash);
        }
    }
}
