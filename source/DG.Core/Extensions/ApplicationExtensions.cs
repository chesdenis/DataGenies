using System;
using System.Linq;
using System.Text.Json;
using DG.Core.Model.ClusterConfig;
using DG.Core.Orchestrators;

namespace DG.Core.Extensions
{
    public static class ApplicationExtensions
    {
        public static ApplicationUniqueId ParseUniqueId(string application, string instanceName)
        {
            return new ApplicationUniqueId()
            {
                Application = application,
                InstanceName = instanceName,
            };
        }

        public static ApplicationUniqueId ParseUniqueId(string uniqueIdAsString)
        {
            return new ApplicationUniqueId()
            {
                Application = uniqueIdAsString.Split('/')[0],
                InstanceName = uniqueIdAsString.Split('/')[1],
            };
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