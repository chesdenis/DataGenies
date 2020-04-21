namespace DG.Core.ConfigManagers
{
    using DG.Core.Models;

    public interface IClusterConfigManager
    {
        ClusterConfig ReadConfig();

        void WriteConfig(ClusterConfig clusterConfig);
    }
}