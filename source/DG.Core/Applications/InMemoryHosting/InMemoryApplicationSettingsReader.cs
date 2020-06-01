using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DG.Core.Attributes;
using DG.Core.Orchestrators;

namespace DG.Core.Applications.InMemoryHosting
{
    public class InMemoryApplicationSettingsReader : IApplicationSettingsReader
    {
        private readonly InMemoryApplications inMemoryApplications;

        public InMemoryApplicationSettingsReader(InMemoryApplications inMemoryApplications)
        {
            this.inMemoryApplications = inMemoryApplications;
        }

        public object GetSettings(ApplicationUniqueId applicationUniqueId)
        {
            var applicationInstance = this.inMemoryApplications[applicationUniqueId].Instances.First();

            return applicationInstance
                .GetType()
                .GetProperties()
                .First(f => f.GetCustomAttributes(typeof(SettingsAttribute)).Any())
                .GetValue(applicationInstance);
        }

        public IEnumerable<object> GetSharedSettings(ApplicationUniqueId applicationUniqueId)
        {
            var applicationInstance = this.inMemoryApplications[applicationUniqueId].Instances.First();

            var sharedSettings = applicationInstance
                .GetType()
                .GetProperties()
                .Where(f => f.GetCustomAttributes(typeof(SharedSettingsAttribute)).Any());

            return sharedSettings.Select(s => s.GetValue(applicationInstance));
        }
    }
}