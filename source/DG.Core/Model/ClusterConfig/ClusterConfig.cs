using System;

namespace DG.Core.Model.ClusterConfig
{
    public class ClusterConfig
    {
        public DateTime LastUpdateTime { get; set; }

        public string HashMD5 { get; set; }

        public Nodes Nodes { get; set; }

        public ApplicationInstances ApplicationInstances { get; set; }
    }
}