using System;
using System.Linq;
using System.Text.Json;
using DG.Core.Model.ClusterConfig;

namespace DG.Core.Extensions
{
    public static class ApplicationExtensions
    {
        public static string ConstructUniqueId(string application, string instanceName)
        {
            return $"{application}_{instanceName}";
        }

        public static string ConstructUniqueId(this ApplicationInstance applicationInstance)
        {
            return ConstructUniqueId(applicationInstance.Type, applicationInstance.Name);
        }

        public static T ExecuteFunctionWithoutArgs<T>(this object instance, Type attributeType)
        {
            var functionToExecute = instance
                .GetType()
                .GetMethods()
                .First(f => f.GetCustomAttributes(attributeType, true).Any());

            var result = functionToExecute.Invoke(instance, new object[] { });

            if (result is string resultAsString)
            {
                return JsonSerializer.Deserialize<T>(
                    resultAsString,
                    new JsonSerializerOptions() { IgnoreNullValues = true });
            }

            return (T)result;
        }

        public static void ExecuteMethodWithoutArgs(this object instance, Type attributeType)
        {
            var methodToExecute = instance
                .GetType()
                .GetMethods()
                .First(f => f.GetCustomAttributes(attributeType, true).Any());

            methodToExecute.Invoke(instance, new object[] { });
        }
    }
}