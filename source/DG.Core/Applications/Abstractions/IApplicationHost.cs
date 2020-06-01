using System.Collections.Generic;

namespace DG.Core.Orchestrators
{
    public interface IApplicationHost
    {
        IApplicationController ApplicationController { get; }

        IApplicationBuilder ApplicationBuilder { get; }

        IApplicationSettingsReader ApplicationSettingsReader { get; }

        IEnumerable<ApplicationInfo> GetApplications();

        bool CanExecute(string hostingModelName);
    }
}