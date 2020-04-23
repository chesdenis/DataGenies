namespace DG.Core.ConfigManagers
{
    using DG.Core.Models;
    using DG.Core.Repositories;
    using Newtonsoft.Json;

    public class JsonClusterConfigManager : IClusterConfigManager
    {
        private readonly IClusterConfigRepository clusterConfigRepository;

        public JsonClusterConfigManager(IClusterConfigRepository clusterConfigRepository)
        {
            this.clusterConfigRepository = clusterConfigRepository;
        }

        public ClusterConfig GetConfig()
        {
            return this.clusterConfigRepository.GetClusterConfig();
        }

        public string GetConfigAsJson()
        {
            var clusterConfig = this.clusterConfigRepository.GetClusterConfig();
            return JsonConvert.SerializeObject(clusterConfig);
        }

        public void WriteConfig(ClusterConfig clusterConfig)
        {
            this.clusterConfigRepository.UpdateClusterConfig(clusterConfig);
        }

        public void WriteConfigAsJson(string clusterConfigAsJson)
        {
            var clusterConfig = JsonConvert.DeserializeObject<ClusterConfig>(clusterConfigAsJson);
            this.clusterConfigRepository.UpdateClusterConfig(clusterConfig);
        }
    }
}