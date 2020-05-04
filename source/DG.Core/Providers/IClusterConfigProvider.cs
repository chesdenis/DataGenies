using DG.Core.Model.ClusterConfig;

namespace DG.Core.Providers
{
    public interface IClusterConfigProvider
    {
        ApplicationTypesSources GetApplicationTypesSources();

        ApplicationInstances GetApplicationInstances();
    }
}