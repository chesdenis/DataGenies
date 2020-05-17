using DG.Core.Attributes;
using DG.Core.Extensions;
using DG.Core.Orchestrators;

namespace DG.Core.Writers
{
    public class ApplicationInstanceSettingsWriter : IApplicationInstanceSettingsWriter
    {
        public void WriteSettings(object applicationInstance, string settingsAsJson)
        {
            var instanceType = applicationInstance.GetType();

            if (instanceType.HasSettings())
            {
                applicationInstance.WriteSettings(settingsAsJson);
            }       

            if (instanceType.HasSharedSettings())
            {
                applicationInstance.WriteSharedSettings(settingsAsJson);
            }
        }
    }
}