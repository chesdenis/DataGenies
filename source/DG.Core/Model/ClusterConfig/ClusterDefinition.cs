using System;
using DG.Core.Model.Markers;

namespace DG.Core.Model.ClusterConfig
{
    public class ClusterDefinition : IHashComputable, IJsonSerializable
    {
        public string HashMD5 { get; set; }
        
        public DateTime LastUpdateTime { get; set; }
        
        public Hosts Hosts { get; set; }

        public ApplicationInstances ApplicationInstances { get; set; }

        public ApplicationTypesSources ApplicationTypesSources { get; set; }
    }
}