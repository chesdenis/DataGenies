namespace DG.HostApp.Services.ClusterConfigActualizer
{
    using System.Collections.Generic;
    using System.Linq;
    using DG.Core.ConfigManagers;
    using DG.Core.Extensions;
    using DG.Core.Orchestrators;

    public class ClusterConfigActualizerEngine : IClusterConfigActualizerEngine
    {
        private readonly IClusterConfigManager clusterConfigManager;
        private readonly IApplicationOrchestrator applicationOrchestrator;
        private readonly IEnumerable<IInstanceActions> instanceActions;
        private readonly IInstanceDifferenceCalculator instanceDifferenceCalculator;

        public ClusterConfigActualizerEngine(
            IClusterConfigManager clusterConfigManager,
            IApplicationOrchestrator applicationOrchestrator,
            IEnumerable<IInstanceActions> instanceActions,
            IInstanceDifferenceCalculator instanceDifferenceCalculator)
        {
            this.clusterConfigManager = clusterConfigManager;
            this.applicationOrchestrator = applicationOrchestrator;
            this.instanceActions = instanceActions;
            this.instanceDifferenceCalculator = instanceDifferenceCalculator;
        }

        public void Actualize()
        {
            var clusterConfig = this.clusterConfigManager.GetConfig();
            var currentHost = this.clusterConfigManager.GetHost();
            var applicationInstances = clusterConfig.ClusterDefinition.ApplicationInstances;
            var instances = this.applicationOrchestrator.GetInMemoryInstancesData();

            if (instances.Any())
            {
                // stopping or changing amount of applications that are already exist
                foreach (var instance in instances)
                {
                    var clusterInstanceDescription = applicationInstances.FirstOrDefault(app =>
                        instance.Key == ApplicationExtensions.ConstructUniqueId(app.Type, app.Name));

                    // case when instance was deleted from cluster
                    if (clusterInstanceDescription == null)
                    {
                        this.applicationOrchestrator.Stop(instance.Key);
                        this.applicationOrchestrator.UnRegister(instance.Key);
                        continue;
                    }

                    // case when instance was moved to another host
                    if (clusterInstanceDescription.PlacementPolicies.FirstOrDefault(pp => pp == currentHost.Name) == null)
                    {
                        this.applicationOrchestrator.Stop(instance.Key);
                        this.applicationOrchestrator.UnRegister(instance.Key);
                        continue;
                    }

                    // case when amount of instance was changed
                    int nodeInstanceAmountDiference = this.instanceDifferenceCalculator.CalculateDifference(clusterInstanceDescription, currentHost, instance.Value.Count);

                    var instanceAmountCorrectionAction = this.instanceActions.FirstOrDefault(ia => ia.CanExecute(nodeInstanceAmountDiference));

                    if (instanceAmountCorrectionAction != null)
                    {
                        instanceAmountCorrectionAction.Execute(clusterInstanceDescription, nodeInstanceAmountDiference);
                    }
                }
            }

            if (applicationInstances.Any())
            {
                // starting the required applications
                foreach (var application in applicationInstances)
                {
                    var isInstanceExist = instances.ContainsKey(ApplicationExtensions.ConstructUniqueId(application.Type, application.Name));
                    var isHostedOnThisNode = application.PlacementPolicies.Any(x => x == currentHost.Name);

                    if (isHostedOnThisNode && !isInstanceExist)
                    {
                        int nodeInstanceAmount = this.instanceDifferenceCalculator.CalculateDifference(application, currentHost, 0);

                        this.applicationOrchestrator.BuildInstance(application.Type, application.Name, application.Settings?.ToString(), nodeInstanceAmount);
                    }
                }
            }
        }
    }
}
