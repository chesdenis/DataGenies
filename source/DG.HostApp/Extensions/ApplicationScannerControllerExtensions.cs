using DG.Core.Extensions;
using DG.Core.Model.ClusterConfig;

namespace DG.HostApp.Extensions
{
    internal static class ApplicationScannerControllerExtensions
    {
        public static string Scan(this Host host) =>
            $"{host.GetApplicationScannerLocalEndpoint()}/Scan";  
    }
}