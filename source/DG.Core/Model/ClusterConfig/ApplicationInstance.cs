namespace DG.Core.Model.ClusterConfig
{
    using System.Collections.Generic;
    using DG.Core.Model.Markers;

    public class ApplicationInstance : IHashComputable, IJsonSerializable
    {
        public string Name { get; set; }

        public string Type { get; set; }

        public string HostingModel { get; set; }

        public object Settings { get; set; }

        public List<string> PlacementPolicies { get; set; }

        public List<string> Models { get; set; }

        public string Count { get; set; }
    }
}