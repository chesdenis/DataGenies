using System.Collections.Generic;

namespace DG.Core.Orchestrators
{
    public class InMemoryApplication
    {
        public ApplicationInfo ApplicationInfo { get; set; }

        public List<object> Instances { get; set; }
    }
}