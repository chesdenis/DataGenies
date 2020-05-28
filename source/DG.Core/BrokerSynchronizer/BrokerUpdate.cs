namespace DG.Core.BrokerSynchronizer
{
    using System;
    using System.Collections.Generic;
    using DG.Core.Model.ClusterConfig;

    public class BrokerUpdate : IHashComputable
    {
        public string Hash { get; set; }

        public IEnumerable<string> PreviousUpdates { get; set; }

        public DateTime Created { get; set; }

        public BrokerCommand[] BrokerCommands { get; set; }
    }
}
