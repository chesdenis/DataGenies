namespace DG.HostApp.Services
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using DG.Core.ConfigManagers;
    using DG.Core.Extensions;
    using DG.Core.Model.ClusterConfig;
    using DG.Core.Orchestrators;
    using Microsoft.Extensions.Hosting;

    public class ClusterConfigActualizer : IHostedService
    {

        private readonly ClusterConfigManager clusterConfigManager;

        private readonly IApplicationOrchestrator applicationOrchestrator;

        public ClusterConfigActualizer(
            ClusterConfigManager clusterConfigManager,
            IApplicationOrchestrator applicationOrchestrator)
        {
            this.clusterConfigManager = clusterConfigManager;
            this.applicationOrchestrator = applicationOrchestrator;
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
                        var instances = this.applicationOrchestrator.GetInMemoryInstancesData();

                        // stopping or changing amount of applications that are already exist
                        foreach (var instance in instances)
                        {
                            var clusterInstanceDescription = applicationInstances.First(app =>
                                instance.Key == ApplicationExtensions.ConstructUniqueId(app.Type, app.Name));

                            // case when instance was deleted from cluster
                            if (clusterInstanceDescription == null)
                            {
                                this.applicationOrchestrator.Stop(instance.Key);
                                this.applicationOrchestrator.UnRegister(instance.Key);
                                continue;
                            }

                            // case when instance was moved to another host
                            if (clusterInstanceDescription.PlacementPolicies.First(pp => pp == currentHost.Name) == null)
                            {
                                this.applicationOrchestrator.Stop(instance.Key);
                                this.applicationOrchestrator.UnRegister(instance.Key);
                                continue;
                            }

                            // case when amount of instance was changed
                            int nodeInstanceAmountDiference = this.InstanceDifferenceCalculator(clusterInstanceDescription, currentHost, instance.Value.Count);

                            if (nodeInstanceAmountDiference > 0)
                            {
                                this.applicationOrchestrator.BuildInstance(clusterInstanceDescription.Type, clusterInstanceDescription.Name, clusterInstanceDescription.Settings?.ToString(), nodeInstanceAmountDiference);
                            }
                            else if (nodeInstanceAmountDiference < 0)
                            {
                                this.applicationOrchestrator.RemoveInstance(clusterInstanceDescription.Type, clusterInstanceDescription.Name, Math.Abs(nodeInstanceAmountDiference));
                            }
                        }

                        // starting the required applications
                        foreach (var application in applicationInstances)
                        {
                            var isInstanceExist = instances.ContainsKey(ApplicationExtensions.ConstructUniqueId(application.Type, application.Name));
                            var isHostedonThisNode = application.PlacementPolicies.First(x => x == currentHost.Name).Any();

                            if (isHostedonThisNode && !isInstanceExist)
                            {
                                int nodeInstanceAmount = this.InstanceDifferenceCalculator(application, currentHost, 0);

                                this.applicationOrchestrator.BuildInstance(application.Type, application.Name, application.Settings?.ToString(), nodeInstanceAmount);
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

        private int InstanceDifferenceCalculator(ApplicationInstance applicationInstance, Core.Model.ClusterConfig.Host currnetHost, int existingInstanceAmount)
        {
            int.TryParse(applicationInstance.Count, out int count);
            int nodeInstanceAmountDifference = (count / applicationInstance.PlacementPolicies.Count) - existingInstanceAmount;

            var hostPlacementPosition = applicationInstance.PlacementPolicies.IndexOf(currnetHost.Name) + 1;
            var remaindedInstances = count % applicationInstance.PlacementPolicies.Count;

            if (remaindedInstances - hostPlacementPosition >= 0)
            {
                nodeInstanceAmountDifference++;
            }

            return nodeInstanceAmountDifference;
        }
    }
}
