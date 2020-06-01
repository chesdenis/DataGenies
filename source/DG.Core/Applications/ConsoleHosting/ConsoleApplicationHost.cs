using System;
using System.Collections.Generic;

namespace DG.Core.Orchestrators
{
    public class ConsoleApplicationHost : IApplicationHost
    {
        private const string HostingModelName = "Console";

        public ConsoleApplicationHost(
          IApplicationController applicationController,
          IApplicationBuilder applicationBuilder,
          IApplicationSettingsReader applicationSettingsReader)
        {
            this.ApplicationController = applicationController;
            this.ApplicationBuilder = applicationBuilder;
            this.ApplicationSettingsReader = applicationSettingsReader;
        }

        public IApplicationController ApplicationController { get; }

        public IApplicationBuilder ApplicationBuilder { get; }

        public IApplicationSettingsReader ApplicationSettingsReader { get; }

        public bool CanExecute(string hostingModelName)
        {
            return hostingModelName == HostingModelName;
        }

        public IEnumerable<ApplicationInfo> GetApplications()
        {
            throw new NotImplementedException();
        }
    }
}