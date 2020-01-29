using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DataGenies.AspNetCore.DataGeniesCore.Middlewares.Responders;
using DataGenies.AspNetCore.DataGeniesCore.Models;
using Microsoft.AspNetCore.Http;

namespace DataGenies.AspNetCore.DataGeniesUI.Middlewares.Responders
{
    public class IndexResponder : IDataGeniesMiddlewareResponder
    {
        private readonly DataGeniesOptions _options;

        public IndexResponder(DataGeniesOptions options)
        {
            _options = options;
        }
        
        public bool CanExecute(string httpMethod, string path)
        {
            return httpMethod == "GET" && Regex.IsMatch(path, $"^/{_options.RoutePrefix}/?index.html$");
        }

        public async Task Respond(HttpContext httpContext, string path)
        {
            var response = httpContext.Response;
            response.ContentType = "text/html;charset=utf-8";

            using (var stream = IndexStream())
            {
                // Inject arguments before writing to response
                var htmlBuilder = new StringBuilder(new StreamReader(stream).ReadToEnd());
                foreach (var entry in GetIndexArguments())
                {
                    htmlBuilder.Replace(entry.Key, entry.Value);
                }

                await response.WriteAsync(htmlBuilder.ToString(), Encoding.UTF8);
            }
        }

        private static Func<Stream> IndexStream { get; } = () => typeof(IndexResponder).GetTypeInfo().Assembly
            .GetManifestResourceStream("DataGenies.AspNetCore.DataGeniesUI.ClientApp.dist.index.html");

        private IDictionary<string, string> GetIndexArguments()
        {
            return new Dictionary<string, string>()
            {
                { "%(DocumentTitle)", _options.DocumentTitle },
            };
        }
    }
}