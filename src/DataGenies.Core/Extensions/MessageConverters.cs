using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DataGenies.Core.Extensions
{
    public static class MessageExtensions
    {
        public static byte[] ToBytes<T>(this T message)
        {
            var jsonData = JsonSerializer.Serialize(message);
            return Encoding.UTF8.GetBytes(jsonData);
        }

        public static T FromBytes<T>(this byte[] messageAsBytes)
        {
            var jsonData = Encoding.UTF8.GetString(messageAsBytes);
            return JsonSerializer.Deserialize<T>(jsonData);
        }
    }
}