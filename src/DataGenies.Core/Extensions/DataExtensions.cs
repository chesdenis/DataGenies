using System.IO;
using System.IO.Compression;
using System.Text;
using System.Text.Json;

namespace DataGenies.Core.Extensions
{
    public static class DataExtensions
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

        public static byte[] Compress(this byte[] data)
        {
            var output = new MemoryStream();
            using (var dstream = new DeflateStream(output, CompressionLevel.Optimal))
            {
                dstream.Write(data, 0, data.Length);
            }

            return output.ToArray();
        }

        public static byte[] Decompress(this byte[] data)
        {
            var input = new MemoryStream(data);
            var output = new MemoryStream();
            using (var dstream = new DeflateStream(input, CompressionMode.Decompress))
            {
                dstream.CopyTo(output);
            }

            return output.ToArray();
        }
    }
}