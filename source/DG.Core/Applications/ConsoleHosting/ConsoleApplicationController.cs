using System.Collections.Generic;
using DG.Core.Model.Output;

namespace DG.Core.Orchestrators
{
    public class ConsoleApplicationController : IApplicationController
    {
        public IEnumerable<StateReport> GetInstanceState(ApplicationUniqueId applicationUniqueId)
        {
            throw new System.NotImplementedException();
        }

        public void Start(ApplicationUniqueId applicationUniqueId)
        {
            throw new System.NotImplementedException();
        }

        public void Stop(ApplicationUniqueId applicationUniqueId)
        {
            throw new System.NotImplementedException();
        }
    }
}