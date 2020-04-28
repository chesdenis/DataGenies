using DG.Core.Extensions;
using DG.Core.Model.ClusterConfig;

namespace DG.HostApp.Extensions
{
    internal static class ClusterConfigManagerControllerExtensions
    {
        public static string GetConfig(this Host host) =>
            $"{host.GetClusterConfigManagerEndpoint()}/GetConfig";  
        
        public static string WriteConfig(this Host host) =>
            $"{host.GetClusterConfigManagerEndpoint()}/WriteConfig"; 
        
        public static string WriteClusterDefinition(this Host host) =>
            $"{host.GetClusterConfigManagerEndpoint()}/WriteClusterDefinition";
        
        public static string SyncClusterDefinitionAcrossHosts(this Host host) =>
            $"{host.GetClusterConfigManagerEndpoint()}/SyncClusterDefinitionAcrossHosts";
    }
}