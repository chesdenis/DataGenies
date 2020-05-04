using DG.Core.Model.ClusterConfig;
using DG.Core.Providers;
using Microsoft.Extensions.Options;

namespace DG.HostApp.Providers
{
    public class ClusterConfigJsonProvider : IClusterConfigProvider
    {
        private readonly IOptions<ApplicationTypesSources> applicationTypesSources;
        private readonly IOptions<ApplicationInstances> applicationInstances;

        public ClusterConfigJsonProvider(
            IOptions<ApplicationTypesSources> applicationTypesSources,
            IOptions<ApplicationInstances> applicationInstances)
        {
            this.applicationTypesSources = applicationTypesSources;
            this.applicationInstances = applicationInstances;
        }
        
        public ApplicationTypesSources GetApplicationTypesSources() => this.applicationTypesSources.Value;

        public ApplicationInstances GetApplicationInstances() => this.applicationInstances.Value;
    }
}