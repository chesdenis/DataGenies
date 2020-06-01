using System;
using System.Collections.Generic;
using System.Linq;
using DG.Core.Orchestrators;
using DG.Core.Scanners;

namespace DG.Core.Applications.InMemoryHosting
{

    public class InMemoryApplicationBuilder : IApplicationBuilder
    {
        private readonly InMemoryApplications inMemoryApplications;

        private readonly IApplicationController applicationController;

        private readonly IApplicationTypesScanner applicationTypesScanner;

        private readonly IApplicationSettingsWriter applicationSettingsWriter;

        public InMemoryApplicationBuilder(
            InMemoryApplications inMemoryApplications,
            IApplicationController applicationController,
            IApplicationTypesScanner applicationTypesScanner,
            IApplicationSettingsWriter applicationSettingsWriter)
        {
            this.inMemoryApplications = inMemoryApplications;
            this.applicationController = applicationController;
            this.applicationTypesScanner = applicationTypesScanner;
            this.applicationSettingsWriter = applicationSettingsWriter;
        }

        public void Build(ApplicationUniqueId applicationUniqueId, string propertiesAsJson)
        {
            if (this.inMemoryApplications.ContainsKey(applicationUniqueId))
            {
                throw new ArgumentException("Already registered", nameof(applicationUniqueId));
            }

            this.inMemoryApplications.Add(applicationUniqueId, new InMemoryApplication()
            {
                ApplicationInfo = new ApplicationInfo()
                {
                    PropertiesAsJson = propertiesAsJson,
                    ApplicationUniqueId = applicationUniqueId,
                },
                Instances = new List<object>(),
            });

            this.Scale(applicationUniqueId, 1);
        }

        public void Scale(ApplicationUniqueId applicationUniqueId, int newInstanceCount)
        {
            this.applicationController.Stop(applicationUniqueId);
            this.inMemoryApplications[applicationUniqueId].Instances.Clear();

            var possibleTypes = this.applicationTypesScanner.Scan();
            var instanceType = possibleTypes.First(f => f.Name == applicationUniqueId.Application);
            var propertiesAsJson = this.inMemoryApplications[applicationUniqueId].ApplicationInfo.PropertiesAsJson;

            for (int i = 0; i < newInstanceCount; i++)
            {
                this.CreateInMemoryInstance(applicationUniqueId, possibleTypes, propertiesAsJson);
            }
        }

        public void Remove(ApplicationUniqueId applicationUniqueId)
        {
            if (!this.inMemoryApplications.ContainsKey(applicationUniqueId))
            {
                throw new ArgumentException("Can't unregister this, because it does not register", nameof(applicationUniqueId));
            }

            this.Scale(applicationUniqueId, 0);
            this.Remove(applicationUniqueId);
        }

        private void CreateInMemoryInstance(ApplicationUniqueId applicationUniqueId, IEnumerable<Type> possibleApplicationTypes, string propertiesAsJson)
        {
            var instanceType = possibleApplicationTypes.First(f => f.Name == applicationUniqueId.Application);
            var instance = Activator.CreateInstance(instanceType);

            this.applicationSettingsWriter.WriteSettings(instance, propertiesAsJson);
            this.inMemoryApplications[applicationUniqueId].Instances.Add(instance);
        }
    }
}