namespace DG.Core.Orchestrators
{
    using System.Collections.Generic;
    using DG.Core.Model.Output;

    public interface IApplicationOrchestrator
    {
        void Register(ApplicationInfo applicationInfo);

        void UnRegister(ApplicationUniqueId applicationUniqueId);

        void Start(ApplicationUniqueId applicationUniqueId);

        void Stop(ApplicationUniqueId applicationUniqueId);

        void Scale(ApplicationUniqueId applicationUniqueId, int instanceCount);

        IEnumerable<ApplicationInfo> GetApplications();
    }
}