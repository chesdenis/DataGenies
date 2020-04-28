using System;
using System.Runtime.CompilerServices;
using DG.Core.Model.ClusterConfig;

namespace DG.Core.Extensions
{
    public static class HostExtensions
    {
        public static string[] GetHostListeningAddress(this Host host)
        {
            return host.ListeningUrls.Split(';', StringSplitOptions.RemoveEmptyEntries);
        }

        public static string GetLocalApiAddress(this Host host)
        {
            return $"{host.LocalAddress}/api";
        }

        public static string GetPublicApiAddress(this Host host)
        {
            return $"{host.PublicAddress}/api";
        }

        public static string GetClusterConfigManagerLocalEndpoint(this Host host)
        {
            return $"{host.GetLocalApiAddress()}/ClusterConfigManager";
        }
        
        public static string GetClusterConfigManagerPublicEndpoint(this Host host)
        {
            return $"{host.GetPublicApiAddress()}/ClusterConfigManager";
        }
    }
}