using System;
using DG.Core.Model.Markers;

namespace DG.Core.Model.ClusterConfig
{
    public class ClusterConfig : IHashComputable, IJsonSerializable
    {
        public Host CurrentHost { get; set; }

        public ClusterDefinition ClusterDefinition { get; set; }
    }
}