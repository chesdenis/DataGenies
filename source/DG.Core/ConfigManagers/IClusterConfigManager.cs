namespace DG.Core.ConfigManagers
{
    using DG.Core.Models;

    public interface IClusterConfigManager
    {
        string GetConfigAsJson();

        void WriteConfig(ClusterConfig clusterConfig);
    }
}