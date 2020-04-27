using System.Runtime.CompilerServices;
using DG.Core.Model.ClusterConfig;

namespace DG.Core.Extensions
{
    public static class HostExtensions
    {
        public static string GetHostListeningAddress(this Host host)
        {
            return $"http://{host.HostAddress}:{host.Port}";
        }

        public static string GetHostApiAddress(this Host host)
        {
            return $"{host.GetHostListeningAddress()}/api";
        }

        public static string GetClusterConfigManagerEndpoint(this Host host)
        {
            return $"{host.GetHostListeningAddress()}/api/ClusterConfigManager";
        }
    }
}