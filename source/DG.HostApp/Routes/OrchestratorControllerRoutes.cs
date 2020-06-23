namespace DG.HostApp.Routes
{
    public class OrchestratorControllerRoutes : DgControllerRoutes
    {
        public const string Root = "api/Orchestrator";
        
        public const string GetInstanceState = "GetInstanceState";

        public const string GetInstanceSettings = "GetInstanceSettings";
        
        public override string GetRoot() => Root;
    }
}