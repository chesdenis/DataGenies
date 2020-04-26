using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using DG.Core.Model.ClusterConfig;
using DG.Core.Repositories;

namespace DG.Core.ConfigManagers
{
    public class ClusterConfigManager : IClusterConfigManager
    {
        private readonly IClusterConfigRepository clusterConfigRepository;

        public ClusterConfigManager(IClusterConfigRepository clusterConfigRepository)
        {
            this.clusterConfigRepository = clusterConfigRepository;
        }
        
        public ClusterConfig GetConfig()
        {
            return this.clusterConfigRepository.GetClusterConfig();
        }

        public Nodes GetNodes()
        {
            return this.GetConfig().Nodes;
        }

        public ApplicationInstances GetApplicationInstances()
        {
            return this.GetConfig().ApplicationInstances;
        }

        public string GetConfigAsJson()
        {
            return JsonSerializer.Serialize(this.GetConfig());
        }
        
        public void WriteConfig(ClusterConfig clusterConfig)
        {
            this.clusterConfigRepository.UpdateClusterConfig(clusterConfig);
        }

        public void WriteConfigAsJson(string clusterConfigAsJson)
        {
            var clusterConfig = JsonSerializer.Deserialize<ClusterConfig>(clusterConfigAsJson);
            this.clusterConfigRepository.UpdateClusterConfig(clusterConfig);
        }
        
        public string GetMd5Hash()
        {
            var configInput = this.GetConfigInput();
            return this.GetMd5Hash(configInput);
        }
        
        public bool VerifyMd5Hash()
        {
            var hashToVerify = this.GetConfig().HashMD5;
            var configInput = this.GetConfigInput();
            
            return this.VerifyMd5Hash(configInput, hashToVerify);
        }
        
        private string GetConfigInput()
        {
            var nodes = this.GetNodes();
            var applicationInstances = this.GetApplicationInstances();

            var nodesAsJson = JsonSerializer.Serialize(nodes);
            var applicationInstancesAsJson = JsonSerializer.Serialize(applicationInstances);

            return string.Concat(nodesAsJson, applicationInstancesAsJson);
        }

        private string GetMd5Hash(string input)
        {
            using MD5 md5Hash = MD5.Create();
            
            var data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            var md5AsStringBuilder = new StringBuilder();
            foreach (var t in data)
            {
                md5AsStringBuilder.Append(t.ToString("x2"));
            }

            return md5AsStringBuilder.ToString();
        }
        
        private bool VerifyMd5Hash(string input, string hash)
        {
            var hashOfInput = this.GetMd5Hash(input);
            var comparer = StringComparer.OrdinalIgnoreCase;

            return comparer.Compare(hashOfInput, hash) == 0;
        }
    }
}