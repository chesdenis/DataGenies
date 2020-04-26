using System.IO;
using System.Reflection;
using System.Text.Json;
using DG.Core.Model.ClusterConfig;

namespace DG.Core.Repositories
{
    public class ClusterJsonConfigRepository : IClusterConfigRepository
    {
        private const string ClusterConfigPath = @"dg-cluster.config.json";

        public ClusterConfig GetClusterConfig()
        {
            string clusterConfigAsJson = File.ReadAllText(ClusterConfigPath);
            return JsonSerializer.Deserialize<ClusterConfig>(clusterConfigAsJson);
        }

        public void UpdateClusterConfig(ClusterConfig clusterConfig)
        {
            string clusterConfigAsJson =
                JsonSerializer.Serialize(clusterConfig, new JsonSerializerOptions()
                {
                    WriteIndented = true,
                });
            File.WriteAllText(ClusterConfigPath, clusterConfigAsJson);
        }
    }
}