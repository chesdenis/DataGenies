namespace DG.Core.Extensions
{
    using System;
    using System.Linq;
    using System.Reflection;
    using DG.Core.Attributes;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public static class PropertiesExtensions
    {
        public static void SetValueToPropertyWithAttribute(this object instance, Type attributeType, string propertyValueAsJson)
        {
            var propertyToFill = instance
                .GetType()
                .GetProperties()
                .First(f => f.GetCustomAttributes(attributeType, true).Any());
            var propertyValue = JsonConvert.DeserializeObject(propertyValueAsJson, propertyToFill.PropertyType);

            propertyToFill.SetValue(instance, propertyValue);
        }

        public static object GetValueFromPropertyWithAttribute(this object instance, Type attributeType)
        {
            var propertyToGet = instance
                .GetType()
                .GetProperties()
                .First(f => f.GetCustomAttributes(attributeType, true).Any());

            return propertyToGet.GetValue(instance);
        }

        public static bool HasPropertyAttribute(this Type type, Type attributeType)
        {
            return type.GetProperties()
                .Any(x => x.GetCustomAttributes(attributeType, true).Any());
        }

        public static void SetApplicationSettingsWithPropertyAttribute(this object instance, string settingsValueAsJson)
        {
            var propertiesToFill = instance
                .GetType()
                .GetProperties()
                .Where(f => f.GetCustomAttributes(typeof(PropertiesAttribute)).Any());

            var settingsJObject = JObject.Parse(settingsValueAsJson);

            foreach (var propertyInfo in propertiesToFill)
            {
                var propertyAtribute = propertyInfo.GetCustomAttributes(typeof(PropertiesAttribute)).First() as PropertiesAttribute;
                propertyInfo.SetValue(instance, settingsJObject.GetJTokenByPath(propertyAtribute.Name).ToObject(propertyInfo.PropertyType));
            }
        }
    }
}
