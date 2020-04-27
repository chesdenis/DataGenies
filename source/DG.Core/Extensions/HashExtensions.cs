using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using DG.Core.Model.ClusterConfig;

namespace DG.Core.Extensions
{
    public static class HashExtensions
    {
        public static string CalculateMd5Hash(this IHashComputable hashComputable)
        {
            var objectAsJson = JsonSerializer.Serialize((object)hashComputable);
            
            using MD5 md5Hash = MD5.Create();
            
            var data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(objectAsJson));
            var md5AsStringBuilder = new StringBuilder();
            foreach (var t in data)
            {
                md5AsStringBuilder.Append(t.ToString("x2"));
            }

            return md5AsStringBuilder.ToString();
        }
    }
}