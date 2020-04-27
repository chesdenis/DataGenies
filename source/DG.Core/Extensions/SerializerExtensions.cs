using System.Text.Json;
using DG.Core.Model.Markers;

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
    }
}