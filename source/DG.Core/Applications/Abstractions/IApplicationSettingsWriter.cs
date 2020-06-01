namespace DG.Core.Orchestrators
{
    public interface IApplicationSettingsWriter
    {
        void WriteSettings(object applicationInstance, string settingsAsJson);
    }
}