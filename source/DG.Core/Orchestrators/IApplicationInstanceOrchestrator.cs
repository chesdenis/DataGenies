namespace DG.Core.Orchestrators
{
    using System;
    using System.Collections.Generic;
    using DG.Core.Model.ClusterConfig;

    public interface IApplicationInstanceOrchestrator
    {
        IDictionary<string, object> GetInMemoryInstancesData();

        void CreateSingleInstanceInMemory(string instanceKey, Type instanceType);

        void CreateInstancesInMemory(IDictionary<string, Type> instancesToCreate);

        IDictionary<string, Type> PrepareInstancesDataToCreate(IEnumerable<ApplicationInstance> appInstances);
    }
}
