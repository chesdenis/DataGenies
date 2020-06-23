using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Core.Model.ClusterConfig;
using DG.Core.Model.Dto;
using Microsoft.Extensions.Options;

namespace DG.HostApp.Services.PageServices
{
    public interface IClusterConfigPageService
    {
        Task WriteConfig(
            ClusterConfig clusterConfig,
            IOptions<DG.Core.Model.ClusterConfig.Host> currentHost,
            bool rawView,
            string rawConfigAsJson);

        Task WriteConfig(
           ClusterConfig clusterConfig,
           IOptions<DG.Core.Model.ClusterConfig.Host> currentHost);

        Task<string> ReadConfig(IOptions<Host> currentHost);

        Task<List<ApplicationDto>> ScanAvailableApplications(IOptions<DG.Core.Model.ClusterConfig.Host> currentHost);

        void SyncConfigAcrossNodes(ClusterConfig clusterConfig, IOptions<Host> currentHost);
    }
}
