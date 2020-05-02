using System;
using System.Text.Json;
using DG.Core.Model.Markers;
using Newtonsoft.Json.Linq;

namespace DG.Core.Extensions
{
    public static class SerializerExtensions
    {
        public static string ToJson(this IJsonSerializable jsonObject)
        {
            return JsonSerializer.Serialize((object)jsonObject);
        }

        public static T FromJson<T>(this string jsonData)
        {
            return JsonSerializer.Deserialize<T>(jsonData);
        }

        public static JToken GetJTokenByPath(this string jsonString, string pathToJObject)
        {
            var config = JObject.Parse(jsonString);
            JToken jToken = null;
            var pathToSectionDelimiter = ':';
            var pathElements = pathToJObject.Split(pathToSectionDelimiter);
            for (int i = 0; i < pathElements.Length; i++)
            {
                if (i == 0)
                {
                    jToken = config[pathElements[i]];
                }
                else
                {
                    if (jToken.ToString().StartsWith("["))
                    {
                        jToken = jToken[Convert.ToInt32(pathElements[i])];
                    }
                    else
                    {
                        jToken = jToken[pathElements[i]];
                    }
                }
            }

            return jToken;
        }

        public static object JTokenToObject(this JToken jtoken, string typeName, string assemblyPath)
        {
            return jtoken.ToObject(TypesExtensions.GetInstanceType(typeName, assemblyPath));
        }
    }
}