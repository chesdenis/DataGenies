using System;
using System.Runtime.CompilerServices;
using DG.Core.Model.ClusterConfig;

namespace DG.Core.Extensions
{
    public static class HostExtensions
    {
        public static string[] GetHostListeningAddress(this Host host) => host.ListeningUrls.Split(';', StringSplitOptions.RemoveEmptyEntries);
        
        public static string GetClusterConfigManagerLocalEndpoint(this Host host) => $"{host.GetLocalApiAddress()}/ClusterConfigManager";
        
        public static string GetClusterConfigManagerPublicEndpoint(this Host host) => $"{host.GetPublicApiAddress()}/ClusterConfigManager";
        
        public static string GetApplicationScannerLocalEndpoint(this Host host) => $"{host.GetLocalApiAddress()}/ApplicationScanner";
        
        public static string GetApplicationScannerPublicEndpoint(this Host host) => $"{host.GetPublicApiAddress()}/ApplicationScanner";
        
        private static string GetLocalApiAddress(this Host host) => $"{host.LocalAddress}/api";

        private static string GetPublicApiAddress(this Host host) => $"{host.PublicAddress}/api";
    }
}