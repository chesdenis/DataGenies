namespace DG.HostApp.Services
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Hosting;

    public class ServiceWatcher : IHostedService
    {
        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.Run(() => { Console.WriteLine("ServiceWatcher started"); }, cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.Run(() => { Console.WriteLine("ServiceWatcher stopped"); }, cancellationToken);
        }
    }
}