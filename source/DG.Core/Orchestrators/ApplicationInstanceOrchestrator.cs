namespace DG.Core.Orchestrators
{
    using System;
    using System.Collections.Generic;
    using DG.Core.Extensions;
    using DG.Core.Providers;
    using DG.Core.Scanners;

    public class ApplicationInstanceOrchestrator : IApplicationInstanceOrchestrator
    {
        private readonly IApplicationInstancesScanner instancesScanner;
        private readonly ITypeProvider assemblyTypeProvider;
        private readonly IDictionary<string, object> inMemoryInstances =
            new Dictionary<string, object>();

        public ApplicationInstanceOrchestrator(
            IApplicationInstancesScanner instancesScanner,
            ITypeProvider typeProvider)
        {
            this.instancesScanner = instancesScanner;
            this.assemblyTypeProvider = typeProvider;
        }

        public void CreateSingleInstanceInMemory(string instanceKey, Type instanceType)
        {
            object instance = Activator.CreateInstance(instanceType);
            if (this.inMemoryInstances.ContainsKey(instanceKey))
            {
                throw new ArgumentException("Already registered", nameof(instanceKey));
            }

            this.inMemoryInstances.Add(instanceKey, instance);
        }

        public void CreateInstancesInMemory(IDictionary<string, Type> instancesToCreate)
        {
            foreach (var instanceData in instancesToCreate)
            {
                this.CreateSingleInstanceInMemory(instanceData.Key, instanceData.Value);
            }
        }

        public IDictionary<string, Type> PrepareInstancesDataToCreate()
        {
            var instanesToCreate = new Dictionary<string, Type>();
            this.instancesScanner.Initialize();
            var instancesNamesAndTypes = this.instancesScanner.GetInstancesNamesAndTypes();
            foreach (var instanceName_Type in instancesNamesAndTypes)
            {
                var instanceTypeName = instanceName_Type.Value;
                var instanceType = this.assemblyTypeProvider.GetInstanceType(instanceTypeName);
                instanesToCreate.Add(instanceName_Type.Key, instanceType);
            }

            return instanesToCreate;
        }

        public IDictionary<string, object> GetInMemoryInstancesData()
        {
            return this.inMemoryInstances;
        }
    }
}
