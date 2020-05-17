namespace DG.Core.Writers
{
    public interface IApplicationInstanceSettingsWriter
    {
        void WriteSettings(object applicationInstance, string settingsAsJson);
    }
}