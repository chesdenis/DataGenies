namespace DG.Core.ConfigManagers
{
    using System;
    using System.Security.Cryptography;
    using System.Text;
    using DG.Core.Models;
    using DG.Core.Repositories;
    using DG.Core.Services;
    using Newtonsoft.Json;

    public class JsonClusterConfigManager : IClusterConfigManager
    {
        private readonly IClusterConfigRepository clusterConfigRepository;
        private readonly ISystemClock systemClock;

        public JsonClusterConfigManager(IClusterConfigRepository clusterConfigRepository, ISystemClock systemClock)
        {
            this.clusterConfigRepository = clusterConfigRepository;
            this.systemClock = systemClock;
        }

        public string GetConfigAsJson()
        {
            var clusterConfig = this.clusterConfigRepository.GetClusterConfig();
            return JsonConvert.SerializeObject(clusterConfig);
        }

        public void WriteConfig(ClusterConfig clusterConfig)
        {
            this.clusterConfigRepository.UpdateClusterConfig(
                this.SignClusterConfig(clusterConfig, this.systemClock.Now));
        }

        private ClusterConfig SignClusterConfig(ClusterConfig clusterConfig, DateTime dateTime)
        {
            clusterConfig.LastUpdateTime = dateTime;
            clusterConfig.HashMD5 = string.Empty;
            var clusterConfigHash = this.CreateMD5(JsonConvert.SerializeObject(clusterConfig));
            clusterConfig.HashMD5 = clusterConfigHash;

            return clusterConfig;
        }

        private string CreateMD5(string inputString)
        {
            using (var md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(inputString);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    // "X2" format specifier makes ToString() return result as hexadecimal string with 2 char 
                    sb.Append(hashBytes[i].ToString("X2"));
                }

                return sb.ToString();
            }
        }
    }
}