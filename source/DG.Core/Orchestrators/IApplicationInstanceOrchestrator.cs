namespace DG.Core.Orchestrators
{
    using System.Collections.Generic;

    public interface IApplicationInstanceOrchestrator
    {
        void CreateSingleInstanceInMemory(string instanceKey, string type);

        void CreateInstancesInMemory(IDictionary<string, string> instancesToCreate);

        IDictionary<string, string> PrepareInstancesDataToCreate();
    }
}
