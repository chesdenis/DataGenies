using DG.Core.Attributes;
using DG.Core.Extensions;
using DG.Core.Orchestrators;
using System.Linq;
using System.Text.Json;

namespace DG.Core.Applications.InMemoryHosting
{
    public class InMemoryApplicationSettingsWriter : IApplicationSettingsWriter
    {
        public void WriteSettings(object applicationInstance, string settingsAsJson)
        {
            var propertyToFill = applicationInstance
                 .GetType()
                 .GetProperties()
                 .First(f => f.GetCustomAttributes(typeof(SettingsAttribute), true).Any());

            var jsonDocument = JsonDocument.Parse(settingsAsJson);
            var settingsSectionAsJson = jsonDocument.RootElement.GetProperty("Settings");

            var settings = JsonSerializer.Deserialize(settingsSectionAsJson.GetRawText(), propertyToFill.PropertyType);

            propertyToFill.SetValue(applicationInstance, settings);

            this.WriteSharedSettings(applicationInstance, settingsAsJson);
        }

        private void WriteSharedSettings(object applicationInstance, string settingsAsJson)
        {
            var propertiesToFill = applicationInstance
                .GetType()
                .GetProperties()
                .Where(f => f.GetCustomAttributes(typeof(SharedSettingsAttribute), true).Any())
                .ToList();

            var jsonDocument = JsonDocument.Parse(settingsAsJson);
            var sharedSettingsSectionAsJson = jsonDocument.RootElement.GetProperty("SharedSettings");

            foreach (var propertyToFill in propertiesToFill)
            {
                var pathToSharedSetting = propertyToFill
                    .GetCustomAttributes(typeof(SharedSettingsAttribute), true)
                    .Cast<SharedSettingsAttribute>().First().PathToSharedSettings;

                var sharedSettingAsJson = sharedSettingsSectionAsJson.GetPropertyByPath(pathToSharedSetting);
                var sharedSetting = JsonSerializer.Deserialize(sharedSettingAsJson.GetRawText(), propertyToFill.PropertyType);

                propertyToFill.SetValue(applicationInstance, sharedSetting);
            }
        }
    }
}