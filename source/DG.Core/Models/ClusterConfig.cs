namespace DG.Core.Models
{
    using System;

    public class ClusterConfig
    {
        public DateTime LastUpdateTime;
        public string HashMD5;
        public Node[] Nodes;
        public ApplicationInstance[] ApplicationInstances;
    }

    public class ApplicationInstance
    {
        public string HostName;
        public int Port;
        public string HostingModel;
    }

    public class Node
    {
        public string Name;
        public string Type;
        public string HostingModel;
        public string PlacementPolicies;
        public int Count;
    }
}
