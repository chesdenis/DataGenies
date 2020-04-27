namespace DG.Core.Models
{
    using System;
    using System.Collections.Generic;

    public class ClusterConfig
    {
        public DateTime LastUpdateTime { get; set; }

        public string HashMD5 { get; set; }

        public List<Node> Nodes { get; set; }

        public List<ApplicationInstance> ApplicationInstances { get; set; }
    }

    public class Node
    {
        public string NodeName { get; set; }

        public string HostName { get; set; }

        public ushort Port { get; set; }

        public string HostingModel { get; set; }
    }

    public class ApplicationInstance
    {
        public string Name { get; set; }

        public string Type { get; set; }

        public string HostingModel { get; set; }

        public List<string> PlacementPolicies { get; set; }

        public int Count { get; set; }
    }
}
