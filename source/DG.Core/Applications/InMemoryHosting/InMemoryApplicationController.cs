using System;
using System.Collections.Generic;
using System.Linq;
using DG.Core.Attributes;
using DG.Core.Extensions;
using DG.Core.Model.Output;
using DG.Core.Orchestrators;
using DG.Core.Scanners;

namespace DG.Core.Applications.InMemoryHosting
{
    public class InMemoryApplicationController : IApplicationController
    {
        private readonly InMemoryApplications inMemoryApplications;

        public InMemoryApplicationController(
            InMemoryApplications inMemoryApplications)
        {
            this.inMemoryApplications = inMemoryApplications;
        }

        public IEnumerable<StateReport> GetInstanceState(ApplicationUniqueId applicationUniqueId)
        {
            var instances = this.inMemoryApplications[applicationUniqueId].Instances;

            if (!instances.Any() || !instances.All(x => x.GetType().HasMethodAttribute(typeof(StateReportAttribute))))
            {
                throw new ArgumentOutOfRangeException(
                    nameof(applicationUniqueId), "Can't find any instances");
            }

            foreach (var instance in instances)
            {
                yield return instance.ExecuteFunctionWithoutArgs<StateReport>(typeof(StateReportAttribute));
            }
        }

        public void Start(ApplicationUniqueId applicationUniqueId)
        {
            this.inMemoryApplications[applicationUniqueId].Instances.ForEach(f => f.ExecuteMethodWithoutArgs(typeof(StartAttribute)));
        }

        public void Stop(ApplicationUniqueId applicationUniqueId)
        {
            this.inMemoryApplications[applicationUniqueId].Instances.ForEach(f => f.ExecuteMethodWithoutArgs(typeof(StopAttribute)));
        }
    }
}