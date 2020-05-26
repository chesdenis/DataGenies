namespace DG.HostApp.Services.ClusterConfigActualizer
{
    using DG.Core.Model.ClusterConfig;

    public interface IInstanceDifferenceCalculator
    {
        int CalculateDifference(ApplicationInstance applicationInstance, Host currnetHost, int existingInstanceAmount);
    }
}