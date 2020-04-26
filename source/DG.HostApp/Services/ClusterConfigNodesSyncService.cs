using System;
using System.Threading;
using System.Threading.Tasks;
using DG.Core.ConfigManagers;
using Microsoft.Extensions.Hosting;

namespace DG.HostApp.Services
{
    public class ClusterConfigNodesSyncService : IHostedService
    {
        private readonly IClusterConfigManager clusterConfigManager;

        private bool stopped;

        public ClusterConfigNodesSyncService(IClusterConfigManager clusterConfigManager)
        {
            this.clusterConfigManager = clusterConfigManager;
        }
        
        public Task StartAsync(CancellationToken cancellationToken)
        {
            this.stopped = false;

            Task.Run(
                async () =>
                {
                    while (true)
                    {
                        if (cancellationToken.IsCancellationRequested || this.stopped)
                        {
                            break;
                        }
                        
                        if (!this.clusterConfigManager.VerifyMd5Hash())
                        {
                            var currentHash = this.clusterConfigManager.GetMd5Hash();
                            var currentConfig = this.clusterConfigManager.GetConfig();
                            currentConfig.HashMD5 = currentHash;
                            Console.WriteLine("Detected changes!");
                            this.clusterConfigManager.WriteConfig(currentConfig);
                            Console.WriteLine("Signoff changes!");
                        }
                        else
                        {
                            Console.WriteLine("No changes!");
                        }
                        
                        await Task.Delay(new TimeSpan(0, 0, 5), cancellationToken);
                    }
                }, cancellationToken);

            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            this.stopped = true;

            await Task.CompletedTask;
        }
    }
}