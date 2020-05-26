namespace DG.HostApp.Services.ClusterConfigActualizer
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using DG.Core.Services;
    using Microsoft.Extensions.Hosting;

    public class ClusterConfigActualizerHostedService : IHostedService
    {
        private readonly IDelayService delayService;
        private readonly IClusterConfigActualizerEngine clusterConfigActualizerEngine;

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

                        this.clusterConfigActualizerEngine.Actualize();

                        this.delayService.Waitms(5000);
                    }
                }, cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.Run(() => { Console.WriteLine("Cluster configuration actualizer hosted service stopped"); }, cancellationToken);
        }
    }
}
