using System;
using System.Collections.Generic;
using DG.Core.Model.Output;

namespace DG.Core.Orchestrators
{
    public interface IApplicationOrchestrator
    {
        IEnumerable<StateReport> GetInstanceState(string application, string instanceName);

        void Register(string application, Type applicationType, string instanceName);

        void Start(string application, string instanceName);

        void Stop(string application, string instanceName);
    }
}