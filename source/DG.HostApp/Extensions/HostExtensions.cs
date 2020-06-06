using System;
using DG.Core.Model.ClusterConfig;
using DG.HostApp.Routes;

namespace DG.HostApp.Extensions
{
    public static class HostExtensions
    {
        public static string[] GetHostListeningAddress(this Host host) => host.ListeningUrls.Split(';', StringSplitOptions.RemoveEmptyEntries);
        
        public static string BuildPublicEndpoint<T>(this Host host, string route)
            where T : DgControllerRoutes, new()
        {
            return BuildEndpoint<T>(host, IpAddressType.Public, route);
        }
        
        public static string BuildLocalEndpoint<T>(this Host host, string route)
            where T : DgControllerRoutes, new()
        {
            return BuildEndpoint<T>(host, IpAddressType.Local, route);
        }

        private static string BuildEndpoint<T>(this Host host, IpAddressType addressType, string route)
            where T : DgControllerRoutes, new()
        {
            var controllerRoutes = new T();
            
            var hostAddress = addressType switch
            {
                IpAddressType.Public => $"{host.PublicAddress}",
                IpAddressType.Local => $"{host.LocalAddress}",
                _ => throw new ArgumentOutOfRangeException(nameof(addressType))
            };

            return $"{hostAddress}/{controllerRoutes.GetRoot()}/{route}";
        }
    }
}