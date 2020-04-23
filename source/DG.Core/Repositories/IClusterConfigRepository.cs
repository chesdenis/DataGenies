namespace DG.Core.Repositories
{
    using DG.Core.Models;

    public interface IClusterConfigRepository
    {
        ClusterConfig GetClusterConfig();

        void UpdateClusterConfig(ClusterConfig clusterConfig);
    }
}
