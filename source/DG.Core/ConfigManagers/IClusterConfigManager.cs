using DG.Core.Model.ClusterConfig;

namespace DG.Core.ConfigManagers
{
    public interface IClusterConfigManager
    {
        ClusterConfig GetConfig();
        
        Nodes GetNodes();

        ApplicationInstances GetApplicationInstances();

        string GetConfigAsJson();

        string GetMd5Hash();
        
        bool VerifyMd5Hash();
        
        void WriteConfig(ClusterConfig clusterConfig);

        void WriteConfigAsJson(string clusterConfigAsJson);
    }
}