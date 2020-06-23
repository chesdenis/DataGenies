using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Core.Model.Dto;
using Microsoft.Extensions.Options;

namespace DG.HostApp.Services.PageServices
{
    public interface IOrchestratorPageService
    {
        Task<List<PropertyDTO>> ScanInMemoryApplicationInstanceSettings(
            string applicationType,
            string instanceName,
            IOptions<DG.Core.Model.ClusterConfig.Host> currentHost);
    }
}
