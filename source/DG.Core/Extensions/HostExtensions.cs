using DG.Core.Model.ClusterConfig;

namespace DG.Core.Extensions
{
    public static class HostExtensions
    {
        public static string GetHostListeningAddress(this Host host)
        {
            return $"http://{host.HostAddress}:{host.Port}";
        }
    }
}