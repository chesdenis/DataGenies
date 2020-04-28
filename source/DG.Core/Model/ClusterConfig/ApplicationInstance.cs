using System.Collections.Generic;
using DG.Core.Model.Markers;

namespace DG.Core.Model.ClusterConfig
{
    public class ApplicationInstance : IHashComputable, IJsonSerializable
    {
        public string Name { get; set; }

        public string Type { get; set; }

        public string HostingModel { get; set; }

        public List<string> PlacementPolicies { get; set; }

        public string Count { get; set; }
    }
}