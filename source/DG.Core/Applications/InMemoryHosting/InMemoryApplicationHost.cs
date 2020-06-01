using System.Collections.Generic;
using System.Linq;
using DG.Core.Extensions;
using DG.Core.Orchestrators;

namespace DG.Core.Applications.InMemoryHosting
{
    public class InMemoryApplicationHost : IApplicationHost
    {
        private const string HostingModelName = "InMemory";

        public InMemoryApplicationHost(
            IApplicationController applicationController,
            IApplicationBuilder applicationBuilder,
            IApplicationSettingsReader applicationSettingsReader)
        {
            this.ApplicationController = applicationController;
            this.ApplicationBuilder = applicationBuilder;
            this.ApplicationSettingsReader = applicationSettingsReader;

            this.InMemoryApplications = new InMemoryApplications();
        }

        public InMemoryApplications InMemoryApplications { get; }

        public IApplicationController ApplicationController { get; }

        public IApplicationBuilder ApplicationBuilder { get; }

        public IApplicationSettingsReader ApplicationSettingsReader { get; }

        public bool CanExecute(string hostingModelName)
        {
            return hostingModelName == HostingModelName;
        }

        public IEnumerable<ApplicationInfo> GetApplications()
        {
            var inMemoryInstances = this.InMemoryApplications.GetAllApplication();

            return inMemoryInstances.Select(s => s.ApplicationInfo).ToList();
        }
    }
}