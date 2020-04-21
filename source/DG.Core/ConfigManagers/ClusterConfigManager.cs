namespace DG.Core.ConfigManagers
{
    using System;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Security.Cryptography;
    using System.Text;
    using DG.Core.Models;
    using Newtonsoft.Json;

    public class ClusterConfigManager : IClusterConfigManager
    {
        private const string ClusterConfigPath = @".\dg-cluster.config.json";

        public ClusterConfig ReadConfig()
        {
            string clusterConfigAsJson = File.ReadAllText(ClusterConfigPath);
            var clusterConfig = JsonConvert.DeserializeObject<ClusterConfig>(clusterConfigAsJson);

            return clusterConfig;
        }

        public void WriteConfig(ClusterConfig clusterConfig)
        {
            string clusterConfigAsJson = JsonConvert.SerializeObject(clusterConfig);
            File.WriteAllText(ClusterConfigPath, clusterConfigAsJson);
        }
    }
}