namespace DG.HostApp.Routes
{
    public class OrchestratorControllerRoutes : DgControllerRoutes
    {
        public const string Root = "api/Orchestrator";
        
        public const string GetInstanceState = "GetInstanceState";
        
        public override string GetRoot() => Root;
    }
}