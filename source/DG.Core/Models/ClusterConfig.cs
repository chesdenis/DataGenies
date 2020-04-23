namespace DG.Core.Models
{
    using System;
    using System.Collections.Generic;

    public class ClusterConfig
    {
        public DateTime LastUpdateTime;
        public string HashMD5;
        public List<Node> Nodes;
        public List<ApplicationInstance> ApplicationInstances;
    }

    public class Node
    {
        public string NodeName;
        public string HostName;
        public ushort Port;
        public string HostingModel;
    }

    public class ApplicationInstance
{
        public string Name;
        public string Type;
        public string HostingModel;
        public List<string> PlacementPolicies;
        public int Count;
    }
}
