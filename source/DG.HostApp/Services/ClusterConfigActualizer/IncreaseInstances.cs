namespace DG.HostApp.Services.ClusterConfigActualizer
{
    using System;
    using DG.Core.Model.ClusterConfig;
    using DG.Core.Orchestrators;

    public class IncreaseInstances : IInstanceActions
    {
        private readonly IApplicationOrchestrator applicationOrchestrator;

        public IncreaseInstances(IApplicationOrchestrator applicationOrchestrator)
        {
            this.applicationOrchestrator = applicationOrchestrator;
        }

        public bool CanExecute(int instanceDifference)
        {
            return instanceDifference > 0;
        }

        public void Execute(ApplicationInstance clusterInstanceDescription, int nodeInstanceAmountDiference)
        {
            this.applicationOrchestrator.BuildInstance(
                clusterInstanceDescription.Type,
                clusterInstanceDescription.Name,
                clusterInstanceDescription.Settings?.ToString(),
                Math.Abs(nodeInstanceAmountDiference));
        }
    }
}
