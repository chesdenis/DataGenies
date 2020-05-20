using System.Collections.Generic;
using DG.Core.Model.ClusterConfig;

namespace DG.Core.ConfigManagers
{
    public interface IClusterConfigManager
    {
        ClusterConfig GetConfig();

        Host GetHost();

        ClusterDefinition GetClusterDefinition();

        IEnumerable<string> GetClusterModels();
        
        void WriteConfig(ClusterConfig clusterConfig);

        void WriteClusterDefinition(ClusterDefinition clusterDefinition);
    }
}