using System;
using DG.Core.Model.Markers;

namespace DG.Core.Model.ClusterConfig
{
    public class Host : IHashComputable, IJsonSerializable
    {
        public string Name { get; set; }
        
        public string ListeningUrls { get; set; }
        
        public string LocalAddress { get; set; }
        
        public string PublicAddress { get; set; }
    }
}