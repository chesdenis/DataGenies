﻿namespace DG.Core.Orchestrators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ApplicationOrchestrator : IApplicationOrchestrator
    {
        private readonly IEnumerable<IApplicationBuilder> applicationBuilders;

        public ApplicationOrchestrator(IEnumerable<IApplicationBuilder> applicationBuilders)
        {
            this.applicationBuilders = applicationBuilders;
        }

        public IEnumerable<ApplicationInfo> GetApplications()
        {
            throw new NotImplementedException();
        }

        public void Register(ApplicationInfo applicationInfo)
        {
            var applicationBuilder = this.applicationBuilders.First(f => f.CanExecute(applicationInfo.HostingModel));
            applicationBuilder.Build(applicationInfo.ApplicationUniqueId, applicationInfo.PropertiesAsJson);
        }

        public void Scale(ApplicationUniqueId applicationUniqueId, int instanceCount)
        {
            throw new NotImplementedException();
        }

        public void Start(ApplicationUniqueId applicationUniqueId)
        {
            throw new NotImplementedException();
        }

        public void Stop(ApplicationUniqueId applicationUniqueId)
        {
            throw new NotImplementedException();
        }

        public void UnRegister(ApplicationUniqueId applicationUniqueId)
        {
            throw new NotImplementedException();
        }


        //private readonly IApplicationTypesScanner applicationTypesScanner;
        //private readonly IApplicationInstanceSettingsWriter instanceSettingsWriter;
        //private readonly Dictionary<string, List<object>> inMemoryInstances = new Dictionary<string, List<object>>();

        //private readonly List<Type> possibleApplicationTypes = new List<Type>();

        //public InMemoryApplicationOrchestrator(IApplicationTypesScanner applicationTypesScanner, IApplicationInstanceSettingsWriter instanceSettingsWriter)
        //{
        //    this.applicationTypesScanner = applicationTypesScanner;
        //    this.instanceSettingsWriter = instanceSettingsWriter;
        //}

        //public IDictionary<string, List<object>> GetInMemoryInstancesData() => this.inMemoryInstances;

        //public IEnumerable<StateReport> GetInstanceState(string application, string instanceName)
        //{
        //    var uniqueId = ApplicationExtensions.ConstructUniqueId(application, instanceName);
        //    var instances = this.inMemoryInstances[uniqueId];

        //    if (!instances.Any() || !instances.All(x => x.GetType().HasMethodAttribute(typeof(StateReportAttribute))))
        //    {
        //        throw new ArgumentOutOfRangeException(
        //            nameof(uniqueId), "Can't find any instances");
        //    }

        //    foreach (var instance in instances)
        //    {
        //        yield return instance.ExecuteFunctionWithoutArgs<StateReport>(typeof(StateReportAttribute));
        //    }
        //}

        //public void CollectPossibleApplicationTypes()
        //{
        //    this.possibleApplicationTypes.Clear();

        //    this.possibleApplicationTypes.AddRange(this.applicationTypesScanner.Scan());
        //}

        //public void Register(string application, string instanceName)
        //{
        //    var uniqueId = ApplicationExtensions.ConstructUniqueId(application, instanceName);

        //    if (this.inMemoryInstances.ContainsKey(uniqueId))
        //    {
        //        throw new ArgumentException("Already registered", nameof(uniqueId));
        //    }

        //    this.inMemoryInstances.Add(uniqueId, new List<object>());
        //}

        //public void UnRegister(string application, string instanceName)
        //{
        //    var uniqueId = ApplicationExtensions.ConstructUniqueId(application, instanceName);
        //    this.UnRegister(uniqueId);
        //}

        //public void UnRegister(string instanceUniqueId)
        //{
        //    if (!this.inMemoryInstances.ContainsKey(instanceUniqueId))
        //    {
        //        throw new ArgumentException("Can't unregister this, because it does not register", nameof(instanceUniqueId));
        //    }

        //    this.Stop(instanceUniqueId);

        //    this.inMemoryInstances[instanceUniqueId].Clear();
        //    this.inMemoryInstances.Remove(instanceUniqueId);
        //}

        //public void BuildInstance(string application, string instanceName, string propertiesAsJson, int count = 1)
        //{
        //    var uniqueId = ApplicationExtensions.ConstructUniqueId(application, instanceName);
        //    var instanceType = this.possibleApplicationTypes.First(f => f.Name == application);

        //    for (int i = 0; i < count; i++)
        //    {
        //        var instance = Activator.CreateInstance(instanceType);

        //        this.instanceSettingsWriter.WriteSettings(instance, propertiesAsJson);

        //        this.inMemoryInstances[uniqueId].Add(instance);
        //    }
        //}

        //public void RemoveInstance(string application, string instanceName, int count = 1)
        //{
        //    var uniqueId = ApplicationExtensions.ConstructUniqueId(application, instanceName);

        //    for (int i = 0; i < count; i++)
        //    {
        //        this.inMemoryInstances[uniqueId].RemoveAt(this.inMemoryInstances[uniqueId].Count - 1);
        //    }
        //}

        //public void Start(string application, string instanceName)
        //{
        //    var uniqueId = ApplicationExtensions.ConstructUniqueId(application, instanceName);
        //    this.Start(uniqueId);
        //}

        //public void Stop(string application, string instanceName)
        //{
        //    var uniqueId = ApplicationExtensions.ConstructUniqueId(application, instanceName);
        //    this.Stop(uniqueId);
        //}

        //public void Start(string instanceUniqueId)
        //{
        //    this.inMemoryInstances[instanceUniqueId].ForEach(f => f.ExecuteMethodWithoutArgs(typeof(StartAttribute)));
        //}

        //public void Stop(string instanceUniqueId)
        //{
        //    this.inMemoryInstances[instanceUniqueId].ForEach(f => f.ExecuteMethodWithoutArgs(typeof(StopAttribute)));
        //}
    }
}