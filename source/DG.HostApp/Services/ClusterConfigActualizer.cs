namespace DG.HostApp.Services
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using DG.Core.ConfigManagers;
    using DG.Core.Model.ClusterConfig;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Options;

    public class ClusterConfigActualizer : IHostedService
    {
        private readonly IConfiguration configuration;

        private readonly IOptions<Hosts> clusterNodesCollection;

        private readonly ClusterConfigManager clusterConfigManager;
        public ClusterConfigActualizer(
            IConfiguration configuration,
            IOptions<Hosts> clusterNodesCollection,
            ClusterConfigManager clusterConfigManager)
        {
            this.configuration = configuration;
            this.clusterNodesCollection = clusterNodesCollection;
            this.clusterConfigManager = clusterConfigManager;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.Run(
                () =>
                {
                    while (true)
                    {
                        if (cancellationToken.IsCancellationRequested)
                        {
                            return;
                        }

                        var clusterConfig = this.clusterConfigManager.GetConfig();
                        var currentHost = this.clusterConfigManager.GetHost();
                        var applicationInstances = clusterConfig.ClusterDefinition.ApplicationInstances;
                        foreach (var application in applicationInstances)
                        {
                            if (application.PlacementPolicies.First(x => x == currentHost.Name).Any())
                            {

                            }
                        }

                        Thread.Sleep(5000);
                    }
                }, cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.Run(() => { Console.WriteLine("ServiceWatcher stopped"); }, cancellationToken);
        }
    }
}
