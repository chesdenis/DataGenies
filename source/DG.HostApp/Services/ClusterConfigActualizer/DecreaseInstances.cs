namespace DG.HostApp.Services.ClusterConfigActualizer
{
    using DG.Core.Model.ClusterConfig;
    using DG.Core.Orchestrators;

    public class DecreaseInstances : IInstanceActions
    {
        private readonly IApplicationOrchestrator applicationOrchestrator;

        public DecreaseInstances(IApplicationOrchestrator applicationOrchestrator)
        {
            this.applicationOrchestrator = applicationOrchestrator;
        }

        public bool CanExecute(int instanceDifference)
        {
            return instanceDifference < 0;
        }

        public void Execute(ApplicationInstance clusterInstanceDescription, int nodeInstanceAmountDiference)
        {
            this.applicationOrchestrator.RemoveInstance(
                clusterInstanceDescription.Type,
                clusterInstanceDescription.Name,
                nodeInstanceAmountDiference);
        }
    }
}
