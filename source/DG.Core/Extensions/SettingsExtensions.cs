namespace DG.Core.Extensions
{
    using System;
    using System.Linq;
    using System.Text.Json;
    using DG.Core.Attributes;

    public static class SettingsExtensions
    {
        public static void WriteSettings(this object instance, string propertyValueAsJson)
        {
            var propertyToFill = instance
                .GetType()
                .GetProperties()
                .First(f => f.GetCustomAttributes(typeof(SettingsAttribute), true).Any());

            var jsonDocument = JsonDocument.Parse(propertyValueAsJson);
            var settingsAsJson = jsonDocument.RootElement.GetProperty("Settings");

            var settings = JsonSerializer.Deserialize(settingsAsJson.GetRawText(), propertyToFill.PropertyType);

            propertyToFill.SetValue(instance, settings);

            WriteSharedSettings(instance, propertyValueAsJson);
        }

        public static void WriteSharedSettings(this object instance, string propertyValueAsJson)
        {
            var propertiesToFill = instance
                .GetType()
                .GetProperties()
                .Where(f => f.GetCustomAttributes(typeof(SharedSettingsAttribute), true).Any())
                .ToList();

            var jsonDocument = JsonDocument.Parse(propertyValueAsJson);
            var sharedSettingsAsJson = jsonDocument.RootElement.GetProperty("SharedSettings");

            foreach (var propertyToFill in propertiesToFill)
            {
                var pathToSharedSetting = propertyToFill
                    .GetCustomAttributes(typeof(SharedSettingsAttribute), true)
                    .Cast<SharedSettingsAttribute>().First().PathToSharedSettings;

                var sharedSettingAsJson = sharedSettingsAsJson.GetPropertyByPath(pathToSharedSetting);
                var sharedSetting = JsonSerializer.Deserialize(sharedSettingAsJson.GetRawText(), propertyToFill.PropertyType);

                propertyToFill.SetValue(instance, sharedSetting);
            }
        }

        public static object ReadSettings(this object instance, Type attributeType)
        {
            var propertyToGet = instance
                .GetType()
                .GetProperties()
                .First(f => f.GetCustomAttributes(attributeType, true).Any());

            return propertyToGet.GetValue(instance);
        }

        public static bool HasSettings(this Type type)
        {
            return type.HasSettings(typeof(SettingsAttribute));
        }

        public static bool HasSharedSettings(this Type type)
        {
            return type.HasSettings(typeof(SharedSettingsAttribute));
        }

        public static bool HasSettings(this Type type, Type attributeType)
        {
            return type.GetProperties()
                .Any(x => x.GetCustomAttributes(attributeType, true).Any());
        }
    }
}
