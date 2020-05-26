namespace DG.HostApp.Services.ClusterConfigActualizer
{
    using DG.Core.Model.ClusterConfig;

    public class InstanceDifferenceCalculator : IInstanceDifferenceCalculator
    {
        public int CalculateDifference(ApplicationInstance applicationInstance, Host currnetHost, int existingInstanceAmount)
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
