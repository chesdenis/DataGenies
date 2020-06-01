using System.Collections.Generic;

namespace DG.Core.Orchestrators
{
    public interface IApplicationSettingsReader
    {
        object GetSettings(ApplicationUniqueId applicationUniqueId);

        IEnumerable<object> GetSharedSettings(ApplicationUniqueId applicationUniqueId);
    }
}