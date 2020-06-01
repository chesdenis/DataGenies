using System.Collections.Generic;
using DG.Core.Model.Output;

namespace DG.Core.Orchestrators
{
    public interface IApplicationController
    {
        void Start(ApplicationUniqueId applicationUniqueId);

        void Stop(ApplicationUniqueId applicationUniqueId);

        IEnumerable<StateReport> GetInstanceState(ApplicationUniqueId applicationUniqueId);
    }
}