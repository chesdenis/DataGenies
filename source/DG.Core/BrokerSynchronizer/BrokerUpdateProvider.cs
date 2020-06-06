namespace DG.Core.BrokerSynchronizer
{
    using DG.Core.Repositories;

    public class BrokerUpdateProvider
    {
        private readonly IClusterConfigRepository clusterConfigRepository;

        public BrokerUpdateProvider(IClusterConfigRepository clusterConfigRepository)
        {
            this.clusterConfigRepository = clusterConfigRepository;
        }

        public BrokerUpdate GetUpdate(string updateHash)
        {
            var hosts = this.clusterConfigRepository.GetClusterConfig().ClusterDefinition.Hosts;
            foreach (var targetHost in hosts)
            {
                // CreateEndpoint for host and access thrue 
                // targetHost.PublicAddress
                var update = (BrokerUpdate) null;

                if (update != null)
                {
                    return update;
                }
            }

            return null;
        }
    }
}
