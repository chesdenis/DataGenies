using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DG.Core.Attributes;
using DG.Core.Extensions;
using DG.Core.Orchestrators;

namespace DG.Core.Readers
{
    public class InMemoryApplicationInstanceSettingsReader : IApplicationInstanceSettingsReader
    {
        private readonly InMemoryApplicationOrchestrator applicationOrchestrator;

        public InMemoryApplicationInstanceSettingsReader(IApplicationOrchestrator applicationOrchestrator)
        {
            this.applicationOrchestrator = (InMemoryApplicationOrchestrator)applicationOrchestrator;
        }

        public IEnumerable<object> GetSharedSettings(string application, string instanceName)
        {
            var uniqueId = ApplicationExtensions.ConstructUniqueId(application, instanceName);

            var applicationInstance = this.applicationOrchestrator.GetInMemoryInstancesData()[uniqueId].First();

            var sharedSettings = applicationInstance
                .GetType()
                .GetProperties()
                .Where(f => f.GetCustomAttributes(typeof(SharedSettingsAttribute)).Any());

            return sharedSettings.Select(s => s.GetValue(applicationInstance));
        }

        public object GetSettings(string application, string instanceName)
        {
            var uniqueId = ApplicationExtensions.ConstructUniqueId(application, instanceName);

            var applicationInstance = this.applicationOrchestrator.GetInMemoryInstancesData()[uniqueId].First();
            
            return applicationInstance
                .GetType()
                .GetProperties()
                .First(f => f.GetCustomAttributes(typeof(SettingsAttribute)).Any())
                .GetValue(applicationInstance);
        }
    }
}