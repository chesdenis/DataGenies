using DG.Core.Model.ClusterConfig;

namespace DG.HostApp.Routes
{
    public class ClusterConfigManagerRoutes : DgControllerRoutes
    {
        public const string Root = "api/ClusterConfigManager";
        
        public const string WriteConfig = "WriteConfig";
        
        public const string GetConfig = "GetConfig";
        
        public const string WriteClusterDefinition = "WriteClusterDefinition";
        
        public override string GetRoot() => Root;
    }
}