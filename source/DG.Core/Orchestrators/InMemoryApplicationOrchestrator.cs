﻿using System;
using System.Collections.Generic;
using System.Linq;
using DG.Core.Attributes;
using DG.Core.Extensions;
using DG.Core.Model.Output;

namespace DG.Core.Orchestrators
{
    public class InMemoryApplicationOrchestrator : IApplicationOrchestrator
    {
        private readonly Dictionary<string, List<object>> inMemoryInstances = new Dictionary<string, List<object>>();

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

        public void Register(string application, Type applicationType, string instanceName)
        {
            var uniqueId = ApplicationExtensions.ConstructUniqueId(application, instanceName);

            if (this.inMemoryInstances.ContainsKey(uniqueId))
            {
                throw new ArgumentException("Already registered", nameof(instanceName));
            }

            this.inMemoryInstances.Add(uniqueId, new List<object>());
            this.inMemoryInstances[uniqueId].Add(Activator.CreateInstance(applicationType));
        }

        public void Start(string application, string instanceName)
        {
            throw new NotImplementedException();
        }

        public void Stop(string application, string instanceName)
        {
            throw new NotImplementedException();
        }
    }
}