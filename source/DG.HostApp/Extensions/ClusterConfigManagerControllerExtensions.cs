using DG.Core.Extensions;
using DG.Core.Model.ClusterConfig;

namespace DG.HostApp.Extensions
{
    internal static class ClusterConfigManagerControllerExtensions
    {
        public static string GetConfig(this Host host) =>
            $"{host.GetClusterConfigManagerLocalEndpoint()}/GetConfig";  
        
        public static string WriteConfig(this Host host) =>
            $"{host.GetClusterConfigManagerLocalEndpoint()}/WriteConfig"; 
        
        public static string WriteClusterDefinition(this Host host) =>
            $"{host.GetClusterConfigManagerLocalEndpoint()}/WriteClusterDefinition";
        
        public static string SyncClusterDefinitionAcrossHosts(this Host host) =>
            $"{host.GetClusterConfigManagerPublicEndpoint()}/SyncClusterDefinitionAcrossHosts";
    }
}