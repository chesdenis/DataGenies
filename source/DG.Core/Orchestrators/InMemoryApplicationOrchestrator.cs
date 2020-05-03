using DG.Core.Scanners;

namespace DG.Core.Orchestrators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using DG.Core.Attributes;
    using DG.Core.Extensions;
    using DG.Core.Model.Output;

    public class InMemoryApplicationOrchestrator : IApplicationOrchestrator
    {
        private readonly IApplicationScanner applicationScanner;
        private readonly Dictionary<string, List<object>> inMemoryInstances = new Dictionary<string, List<object>>();

        private readonly List<Type> possibleApplicationTypes = new List<Type>();

        public InMemoryApplicationOrchestrator(IApplicationScanner applicationScanner)
        {
            this.applicationScanner = applicationScanner;
        }

        public IDictionary<string, List<object>> GetInMemoryInstancesData() => this.inMemoryInstances;

        public IEnumerable<StateReport> GetInstanceState(string application, string instanceName)
        {
            var uniqueId = ApplicationExtensions.ConstructUniqueId(application, instanceName);
            var instances = this.inMemoryInstances[uniqueId];

            if (!instances.Any() || !instances.All(x => x.GetType().HasMethodAttribute(typeof(StateReportAttribute))))
            {
                throw new ArgumentOutOfRangeException(
                    nameof(uniqueId), "Can't find any instances");
            }

            foreach (var instance in instances)
            {
                yield return instance.ExecuteFunctionWithoutArgs<StateReport>(typeof(StateReportAttribute));
            }
        }

        public IEnumerable<PropertyInfo> GetSettingsProperties(string application, string instanceName)
        {
            var uniqueId = ApplicationExtensions.ConstructUniqueId(application, instanceName);
            return this.inMemoryInstances[uniqueId]
                .First()
                .GetType()
                .GetProperties()
                .Where(f => f.GetCustomAttributes(typeof(PropertyAttribute)).Any());
        }

        public void CollectPossibleApplicationTypes()
        {
            this.possibleApplicationTypes.Clear();
            
            this.possibleApplicationTypes.AddRange(this.applicationScanner.Scan());
        }

        public void Register(string application, string instanceName)
        {
            var uniqueId = ApplicationExtensions.ConstructUniqueId(application, instanceName);

            if (this.inMemoryInstances.ContainsKey(uniqueId))
            {
                throw new ArgumentException("Already registered", nameof(uniqueId));
            }
    
            this.inMemoryInstances.Add(uniqueId, new List<object>());
        }

        public void UnRegister(string application, string instanceName)
        {
            var uniqueId = ApplicationExtensions.ConstructUniqueId(application, instanceName);
            
            if (!this.inMemoryInstances.ContainsKey(uniqueId))
            {
                throw new ArgumentException("Can't unregister this, because it does not register", nameof(uniqueId));
            }

            this.Stop(application, instanceName);
            
            this.inMemoryInstances[uniqueId].Clear();
            this.inMemoryInstances.Remove(uniqueId);
        }

        public void BuildInstance(string application, string instanceName, string propertiesAsJson, int count = 1)
        {
            var uniqueId = ApplicationExtensions.ConstructUniqueId(application, instanceName);
            var instanceType = this.possibleApplicationTypes.First(f => f.Name == application);
            
            for (int i = 0; i < count; i++)
            {
                var instance = Activator.CreateInstance(instanceType);

                if (instanceType.HasPropertyAttribute(typeof(PropertiesAttribute)))
                {
                    instance.SetValueToPropertyWithAttribute(typeof(PropertiesAttribute), propertiesAsJson);
                }

                this.inMemoryInstances[uniqueId].Add(instance);
            }
        }

        public void Start(string application, string instanceName)
        {
            var uniqueId = ApplicationExtensions.ConstructUniqueId(application, instanceName);
            this.inMemoryInstances[uniqueId].ForEach(f => f.ExecuteMethodWithoutArgs(typeof(StartAttribute)));
        }

        public void Stop(string application, string instanceName)
        {
            var uniqueId = ApplicationExtensions.ConstructUniqueId(application, instanceName);
            this.inMemoryInstances[uniqueId].ForEach(f => f.ExecuteMethodWithoutArgs(typeof(StopAttribute)));
        }
    }
}