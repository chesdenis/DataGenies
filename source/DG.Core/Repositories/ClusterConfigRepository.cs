namespace DG.Core.Repositories
{
    using System.IO;
    using DG.Core.Models;
    using Newtonsoft.Json;

    // this class need to be fully reworked
    public class ClusterConfigRepository : IClusterConfigRepository
    {
        private const string ClusterConfigPath = @".\dg-cluster.config.json";

        public ClusterConfig GetClusterConfig()
        {
            string clusterConfigAsJson = File.ReadAllText(ClusterConfigPath);
            return JsonConvert.DeserializeObject<ClusterConfig>(clusterConfigAsJson);
        }

        public void UpdateClusterConfig(ClusterConfig clusterConfig)
        {
            string clusterConfigAsJson = JsonConvert.SerializeObject(clusterConfig);
            File.WriteAllText(ClusterConfigPath, clusterConfigAsJson);
        }
    }
}
