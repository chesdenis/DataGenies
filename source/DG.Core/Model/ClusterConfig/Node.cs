namespace DG.Core.Model.ClusterConfig
{
    public class Node
    {
        public string NodeName { get; set; }
        
        public string HostName { get; set; }
        
        public ushort Port { get; set; }
        
        public string HostingModel { get; set; }
    }
}