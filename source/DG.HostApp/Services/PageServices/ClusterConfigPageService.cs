using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using DG.Core.Extensions;
using DG.Core.Model.ClusterConfig;
using DG.Core.Model.Dto;
using DG.Core.Services;
using DG.HostApp.Extensions;
using DG.HostApp.Routes;
using Microsoft.Extensions.Options;

namespace DG.HostApp.Services.PageServices
{
    public class ClusterConfigPageService : IClusterConfigPageService
    {
        private IHttpService httpService;

        public ClusterConfigPageService(IHttpService httpService)
        {
            this.httpService = httpService;
        }

        public async Task WriteConfig(
            ClusterConfig clusterConfig,
            IOptions<DG.Core.Model.ClusterConfig.Host> currentHost,
            bool rawView,
            string rawConfigAsJson)
        {
            if (rawView == false)
            {
                rawConfigAsJson = JsonSerializer.Serialize(clusterConfig, new JsonSerializerOptions() { WriteIndented = true });
            }

            await this.httpService.Post(currentHost.Value.BuildLocalEndpoint<ClusterConfigManagerRoutes>(ClusterConfigManagerRoutes.WriteConfig), rawConfigAsJson);

            this.SyncConfigAcrossNodes(clusterConfig, currentHost);
        }

        public async Task WriteConfig(
           ClusterConfig clusterConfig,
           IOptions<DG.Core.Model.ClusterConfig.Host> currentHost)
        {
            var rawConfigAsJson = JsonSerializer.Serialize(clusterConfig, new JsonSerializerOptions() { WriteIndented = true });
            await this.httpService.Post(currentHost.Value.BuildLocalEndpoint<ClusterConfigManagerRoutes>(ClusterConfigManagerRoutes.WriteConfig), rawConfigAsJson);
            this.SyncConfigAcrossNodes(clusterConfig, currentHost);
        }

        public async Task<string> ReadConfig(IOptions<Host> currentHost)
        {
            return await this.httpService.Get(currentHost.Value.BuildLocalEndpoint<ClusterConfigManagerRoutes>(ClusterConfigManagerRoutes.GetConfig));
        }

        public async Task<List<ApplicationDto>> ScanAvailableApplications(IOptions<DG.Core.Model.ClusterConfig.Host> currentHost)
        {
            var availableApplicationsAsJson = await this.httpService.Get(currentHost.Value.BuildLocalEndpoint<ApplicationScannerControllerRoutes>(ApplicationScannerControllerRoutes.Scan));
            return JsonSerializer.Deserialize<List<ApplicationDto>>(availableApplicationsAsJson, new JsonSerializerOptions() { WriteIndented = true, IgnoreNullValues = true });
        }

        public void SyncConfigAcrossNodes(ClusterConfig clusterConfig, IOptions<Host> currentHost)
        {
            Parallel.ForEach(clusterConfig.ClusterDefinition.Hosts, async (host) =>
            {
                if (host.Name.ToLowerInvariant() == currentHost.Value.Name.ToLowerInvariant())
                {
                    return;
                }

                try
                {
                    await Task.Run(() => this.httpService.Post(
                        host.BuildPublicEndpoint<ClusterConfigManagerRoutes>(ClusterConfigManagerRoutes.WriteClusterDefinition),
                        clusterConfig.ClusterDefinition.ToJson()));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            });
        }
    }
}
