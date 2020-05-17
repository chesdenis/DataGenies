namespace DG.HostApp.Routes
{
    public class ApplicationScannerControllerRoutes : DgControllerRoutes
    {
        public const string Root = "api/ApplicationScanner";
        
        public const string Scan = "Scan";
        
        public override string GetRoot() => Root;
    }
}