namespace DG.Core.Orchestrators
{
    using System;
    using System.Collections.Generic;
    using DG.Core.Model.Output;

    public interface IApplicationOrchestrator
    {
        IEnumerable<StateReport> GetInstanceState(string application, string instanceName);

        object GetProperties(string application, string instanceName);

        void Register(string application, Type applicationType, string instanceName);

        void Register(string application, Type applicationType, string instanceName, string propertiesAsJson);

        void Start(string application, string instanceName);

        void Stop(string application, string instanceName);
    }
}