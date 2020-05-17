namespace DG.Core.Extensions
{
    using System;
    using System.Text.Json;
    using DG.Core.Model.Markers;

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

        public static JsonElement GetPropertyByPath(this JsonElement jsonElement, string propertyPath)
        {
            var propertyPathParts = propertyPath.Split(':');

            // JsonElement is a struct, so this copying operation.
            var retVal = jsonElement;
            foreach (var propertyPathPart in propertyPathParts)
            {
                retVal = retVal.GetProperty(propertyPathPart);
            }

            return retVal;
        }

        // public static JToken GetJTokenByPath(this JObject jObject, string pathToJObject)
        // {
        //     var pathToSectionDelimiter = ':';
        //     var pathElements = pathToJObject.Split(pathToSectionDelimiter);
        //     JToken jToken = jObject[pathElements[0]];
        //     for (int i = 1; i < pathElements.Length; i++)
        //     {
        //         {
        //             if (jToken.ToString().StartsWith("["))
        //             {
        //                 jToken = jToken[Convert.ToInt32(pathElements[i])];
        //                 if (jToken == null)
        //                 {
        //                     throw new ArgumentException($"The path {pathElements[i]} is invalid");
        //                 }
        //             }
        //             else
        //             {
        //                 jToken = jToken[pathElements[i]];
        //                 if (jToken == null)
        //                 {
        //                     throw new ArgumentException($"The path {pathElements[i]} is invalid");
        //                 }
        //             }
        //         }
        //     }
        //
        //     return jToken;
        // }

        // public static T JTokenToObject<T>(this JToken jtoken)
        // {
        //     return jtoken.ToObject<T>();
        // }
    }
}