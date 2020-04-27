namespace DG.HostApp.Services
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using DG.Core.Model.ClusterConfig;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Options;

    public class ServiceWatcher : IHostedService
    {
        private readonly IConfiguration configuration;

        private readonly IOptions<Hosts> clusterNodesCollection;

        public ServiceWatcher(IConfiguration configuration, IOptions<Hosts> clusterNodesCollection)
        {
            this.configuration = configuration;
            this.clusterNodesCollection = clusterNodesCollection;
        }
        
        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.Run(
                () =>
            {
                var nodes = this.clusterNodesCollection.Value;
                nodes.ForEach(x => Console.WriteLine(x.Name));
            }, cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.Run(() => { Console.WriteLine("ServiceWatcher stopped"); }, cancellationToken);
        }
    }
}