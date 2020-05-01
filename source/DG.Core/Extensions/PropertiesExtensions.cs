namespace DG.Core.Extensions
{
    using System;
    using System.Linq;
    using Newtonsoft.Json;

    public static class PropertiesExtensions
    {
        public static void SetValueToPropertyWithAttribute(this object instance, Type attributeType, string propertyValueAsJson)
        {
            var test1 = instance
                .GetType()
                .GetProperties();

            var test2 = instance
                .GetType();

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
    }
}
