namespace DG.Core.Orchestrators
{
    using System.Collections.Generic;
    using DG.Core.Model.Output;

    public interface IApplicationOrchestrator
    {
        IEnumerable<StateReport> GetInstanceState(string application, string instanceName);

        IDictionary<string, List<object>> GetInMemoryInstancesData();

        void CollectPossibleApplicationTypes();

        void Register(string application, string instanceName);

        void UnRegister(string instanceUniqueId);

        void UnRegister(string application, string instanceName);

        void BuildInstance(string application, string instanceName, string propertiesAsJson, int count = 1);

        public void RemoveInstance(string application, string instanceName, int count = 1);

        void Start(string instanceUniqueId);

        void Stop(string instanceUniqueId);

        void Start(string application, string instanceName);

        void Stop(string application, string instanceName);
    }
}