using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly ISystemClock systemClock;

        private ClusterConfig configCache;

        public ClusterConfigManager(IClusterConfigRepository clusterConfigRepository, IHttpService httpService, ISystemClock systemClock)
        {
            this.clusterConfigRepository = clusterConfigRepository;
            this.httpService = httpService;
            this.systemClock = systemClock;
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

        public IEnumerable<string> GetClusterModels()
        {
            return this.GetConfig().ClusterDefinition.ApplicationInstances.SelectMany(s => s.Models).Distinct();
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
                currentConfig.ClusterDefinition.LastUpdateTime = this.systemClock.Now;
                
                this.WriteConfig(currentConfig);
            }
        }
    }
}