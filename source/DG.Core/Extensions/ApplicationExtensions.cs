using System;
using System.Linq;
using Newtonsoft.Json;

namespace DG.Core.Extensions
{
    public static class ApplicationExtensions
    {
        public static string ConstructUniqueId(string application, string instanceName)
        {
            return $"{application}/{instanceName}";
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
                return JsonConvert.DeserializeObject<T>(resultAsString);
            }

            return (T)result;
        }
    }
}