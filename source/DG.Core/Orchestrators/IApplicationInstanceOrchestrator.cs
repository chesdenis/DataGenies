namespace DG.Core.Orchestrators
{
    using System;
    using System.Collections.Generic;

    public interface IApplicationInstanceOrchestrator
    {
        IDictionary<string, object> GetInMemoryInstancesData();

        void CreateSingleInstanceInMemory(string instanceKet, Type instanceType);

        void CreateInstancesInMemory(IDictionary<string, Type> instancesToCreate);

        IDictionary<string, Type> PrepareInstancesDataToCreate();
    }
}
