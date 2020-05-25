namespace DG.HostApp.Services.ClusterConfigActualizer
{
    using DG.Core.Model.ClusterConfig;

    public interface IInstanceActions
    {
        bool CanExecute(int instanceDifference);

        void Execute(ApplicationInstance clusterInstanceDescription, int nodeInstanceAmountDiference);
    }
}
