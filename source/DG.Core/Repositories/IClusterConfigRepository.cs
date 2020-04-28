using DG.Core.Model.ClusterConfig;

namespace DG.Core.Repositories
{
    public interface IClusterConfigRepository
    {
        ClusterConfig GetClusterConfig();

        void UpdateClusterConfig(ClusterConfig clusterConfig);
    }
}