using System;
using System.Threading.Tasks;
using DG.Core.Extensions;
using DG.Core.Model.ClusterConfig;
using DG.Core.Repositories;
using DG.Core.Services;

namespace DG.Core.ConfigManagers
{
    public class ClusterConfigManager : IClusterConfigManager
    {
        private readonly IClusterConfigRepository clusterConfigRepository;
        private readonly IHttpService httpService;

        private ClusterConfig configCache;

        public ClusterConfigManager(IClusterConfigRepository clusterConfigRepository, IHttpService httpService)
        {
            this.clusterConfigRepository = clusterConfigRepository;
            this.httpService = httpService;
        }
        
        public ClusterConfig GetConfig()
        {
            try
            {
                var clusterConfig = this.clusterConfigRepository.GetClusterConfig();
                if (clusterConfig != null)
                {
                    this.configCache = clusterConfig;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                if (this.configCache == null)
                {
                    throw;
                }
            }
            
            return this.configCache;
        }

        public Host GetHost()
        {
            return this.GetConfig().CurrentHost;
        }

        public ClusterDefinition GetClusterDefinition()
        {
            return this.GetConfig().ClusterDefinition;
        }

        public void WriteConfig(ClusterConfig clusterConfig)
        {
            this.clusterConfigRepository.UpdateClusterConfig(clusterConfig);
        }

        public void WriteClusterDefinition(ClusterDefinition clusterDefinition)
        {
            var currentConfig = this.GetConfig();

            var currentClusterDefinitionHash = currentConfig.ClusterDefinition.CalculateMd5Hash();
            var newClusterDefinitionHash = clusterDefinition.CalculateMd5Hash();

            if (currentClusterDefinitionHash != newClusterDefinitionHash)
            {
                currentConfig.ClusterDefinition = clusterDefinition;
                currentConfig.ClusterDefinition.HashMD5 = clusterDefinition.CalculateMd5Hash();
                currentConfig.ClusterDefinition.LastUpdateTime = DateTime.UtcNow;
                
                this.WriteConfig(currentConfig);
            }
        }

        public void SyncConfigsAcrossHosts()
        {
            var currentHost = this.GetHost();
            
            foreach (var host in this.GetConfig().ClusterDefinition.Hosts)
            {
                if (host.Name.ToLowerInvariant() == currentHost.Name.ToLowerInvariant())
                {
                    continue;
                }
            
                try
                {
                    Task.Run(() => this.httpService.Post(
                        $"http://{host.HostAddress}:{host.Port}/WriteClusterDefinition",
                        this.GetClusterDefinition().ToJson()));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }
    }
}