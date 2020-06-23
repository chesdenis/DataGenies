using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DG.Core.Extensions;
using DG.Core.Model.Dto;
using DG.Core.Orchestrators;
using DG.Core.Providers;
using DG.Core.Services;
using DG.HostApp.Extensions;
using DG.HostApp.Routes;
using Microsoft.Extensions.Options;

namespace DG.HostApp.Services.PageServices
{
    public class OrchestratorPageService : IOrchestratorPageService
    {
        private readonly IHttpService httpService;
        public OrchestratorPageService(IHttpService httpService)
        {
            this.httpService = httpService;
        }

        public async Task<List<PropertyDTO>> ScanInMemoryApplicationInstanceSettings(string applicationType, string instanceName, IOptions<DG.Core.Model.ClusterConfig.Host> currentHost)
        {
            var queryString = new StringBuilder();
            string[] queryParams = new string[] { "?", "appType=", applicationType, "&instanceName=", instanceName };
            queryString.AppendJoin(string.Empty, queryParams);
            var settingsAsString = await this.httpService.Get(
             currentHost.Value.BuildLocalEndpoint<OrchestratorControllerRoutes>(OrchestratorControllerRoutes.GetInstanceSettings) + queryString);
            return SerializerExtensions.FromJson<List<PropertyDTO>>(settingsAsString);
        }
    }
}
