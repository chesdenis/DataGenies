namespace DG.Core.Orchestrators
{
    using System;
    using System.Collections.Generic;
    using DG.Core.Extensions;
    using DG.Core.Model.ClusterConfig;
    using DG.Core.Providers;

    public class ApplicationInstanceOrchestrator : IApplicationInstanceOrchestrator
    {
        private readonly ITypeProvider assemblyTypeProvider;
        private readonly IDictionary<string, object> inMemoryInstances =
            new Dictionary<string, object>();

        public ApplicationInstanceOrchestrator(
            ITypeProvider typeProvider)
        {
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

        public IDictionary<string, Type> PrepareInstancesDataToCreate(IEnumerable<ApplicationInstance> appInstances, string assemblyPath)
        {
            var instanesToCreate = new Dictionary<string, Type>();
            foreach (var instance in appInstances)
            {
                var instanceKey = ApplicationExtensions.ConstructUniqueId(instance.Type, instance.Name);
                var instanceType = this.assemblyTypeProvider.GetInstanceType(instance.Type, assemblyPath);
                instanesToCreate.Add(instanceKey, instanceType);
            }

            return instanesToCreate;
        }

        public IDictionary<string, object> GetInMemoryInstancesData()
        {
            return this.inMemoryInstances;
        }
    }
}
