namespace DG.Core.Orchestrators
{
    using System;
    using System.Collections.Generic;
    using DG.Core.Scanners;

    public class ApplicationInstanceOrchestator : IApplicationInstanceOrchestrator
    {
        private readonly IApplicationInstancesScanner instancesScanner;
        private readonly IAssemblyScanner assemblyScanner;
        private readonly IDictionary<string, object> inMemoryInstances =
            new Dictionary<string, object>();

        private string namespaceName;
        private string assemblyPath;

        public ApplicationInstanceOrchestator(
            IApplicationInstancesScanner instancesScanner,
            IAssemblyScanner assemblyScanner,
            string assemblyPath,
            string namespaceName)
        {
            this.assemblyScanner = assemblyScanner;
            this.instancesScanner = instancesScanner;
            this.assemblyPath = assemblyPath;
            this.namespaceName = namespaceName;
        }

        public void CreateSingleInstanceInMemory(string instanceKey, string type)
        {
            var instanceType = this.assemblyScanner.GetType(this.assemblyPath, this.namespaceName, type);
            object instance = Activator.CreateInstance(instanceType);
            if (this.inMemoryInstances.ContainsKey(instanceKey))
            {
                throw new ArgumentException("Already registered", nameof(instanceKey));
            }

            this.inMemoryInstances.Add(instanceKey, instance);
        }

        public void CreateInstancesInMemory(IDictionary<string, string> instancesToCreate)
        {
            foreach (var instanceData in instancesToCreate)
            {
                this.CreateSingleInstanceInMemory(instanceData.Key, instanceData.Value);
            }
        }

        public IDictionary<string, string> PrepareInstancesDataToCreate()
        {
            this.instancesScanner.Initialize();
            return this.instancesScanner.GetInstancesTypeNames();
        }
    }
}
