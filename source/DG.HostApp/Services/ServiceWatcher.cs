using System.Collections;
using System.Collections.Generic;
using DG.Core.Model.ClusterConfig;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace DG.HostApp.Services
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Hosting;

    public class ServiceWatcher : IHostedService
    {
        private readonly IConfiguration configuration;

        private readonly IOptions<Nodes> clusterNodesCollection;

        public ServiceWatcher(IConfiguration configuration, IOptions<Nodes> clusterNodesCollection)
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
                nodes.ForEach(x => Console.WriteLine(x.NodeName));
            }, cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.Run(() => { Console.WriteLine("ServiceWatcher stopped"); }, cancellationToken);
        }
    }
}