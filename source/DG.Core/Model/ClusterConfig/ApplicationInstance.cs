using System.Collections.Generic;

namespace DG.Core.Model.ClusterConfig
{
    public class ApplicationInstance
    {
        public string Name { get; set; }

        public string Type { get; set; }

        public string HostingModel { get; set; }

        public List<string> PlacementPolicies { get; set; }

        public int Count { get; set; }
    }
}