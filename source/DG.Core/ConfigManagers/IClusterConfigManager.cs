namespace DG.Core.ConfigManagers
{
    using DG.Core.Models;

    public interface IClusterConfigManager
    {
        ClusterConfig GetConfig();

        string GetConfigAsJson();

        void WriteConfig(ClusterConfig clusterConfig);

        void WriteConfigAsJson(string clusterConfigAsJson);
    }
}