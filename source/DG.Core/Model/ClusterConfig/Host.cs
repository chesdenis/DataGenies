using System;
using DG.Core.Model.Markers;

namespace DG.Core.Model.ClusterConfig
{
    public class Host : IHashComputable, IJsonSerializable
    {
        public string Name { get; set; }
        
        public string HostAddress { get; set; }
        
        public ushort Port { get; set; }
        
        public string HostingModel { get; set; }
    }
}