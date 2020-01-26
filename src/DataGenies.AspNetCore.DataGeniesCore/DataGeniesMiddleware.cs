using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DataGenies.AspNetCore.DataGeniesCore.Abstraction;
using DataGenies.AspNetCore.DataGeniesCore.Abstraction.Publisher;
using DataGenies.AspNetCore.DataGeniesCore.Attribute;
using DataGenies.AspNetCore.DataGeniesCore.Publisher;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DataGenies.AspNetCore.DataGeniesCore
{
    public class DataGeniesMiddleware
    {
        private const string EmbeddedFileNamespace =
            "DataGenies.AspNetCore.DataGeniesCore.node_modules.datagenies_ui_dist";
        
        private readonly DataGeniesOptions _options;
        private readonly StaticFileMiddleware _staticFileMiddleware;

        public DataGeniesMiddleware(
            RequestDelegate next,
            IWebHostEnvironment webHostEnv,
            ILoggerFactory loggerFactory,
            DataGeniesOptions options
        )
        {
            _options = options ?? new DataGeniesOptions();
            _staticFileMiddleware = CreateStaticFileMiddleware(next,webHostEnv,loggerFactory, options);
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var httpMethod = httpContext.Request.Method;
            var path = httpContext.Request.Path.Value;

            if (httpMethod == "GET" && Regex.IsMatch(path, $"^/{_options.RoutePrefix}/?$"))
            {
                // Use relative redirect to support proxy environments
                var relativeRedirectPath = path.EndsWith("/")
                    ? "index.html"
                    : $"{path.Split('/').Last()}/index.html";

                RespondWithRedirect(httpContext.Response, relativeRedirectPath);
                return;
            }
            
            if (httpMethod == "GET" && Regex.IsMatch(path, $"^/{_options.RoutePrefix}/?index.html$"))
            {
                await RespondWithIndexHtml(httpContext.Response);
                return;
            }

            if (httpMethod == "GET" && Regex.IsMatch(path, $"^/{_options.RoutePrefix}/ApplicationTypes"))
            {
                await RespondWithApplicationTypes(httpContext.Response);
                return;
            }

            await _staticFileMiddleware.Invoke(httpContext);
        }

        private async Task RespondWithApplicationTypes(HttpResponse response)
        {
            response.ContentType = "application/json;charset=utf-8";
            
            var asm = Assembly.LoadFile("C:\\GeniesDropFolder\\TestMicroservice.dll");
            var types = asm.GetTypes()
                .Where(w => w.IsClass)
                .Where(w => w.GetCustomAttributes().Any(ww => ww.GetType() == typeof(ApplicationTypeAttribute)))
                .ToList();

            var firstType = (IRunnable) Activator.CreateInstance(types[0],
                new BasicDataPublisher(new FakePublisher(), new List<IConverter>()));
            
            firstType.Run();
            
            var responseObject = types
                .Select(s =>
                {
                    return new
                    {
                        Name = s.Name,
                        Attributes = s.GetCustomAttributes(false).Select(ss => ss.GetType().Name)
                    };
                }).ToList();
            
            
            
            var responseData = JsonSerializer.Serialize(responseObject);

            await response.WriteAsync(responseData, Encoding.UTF8);
        }

        private async Task RespondWithIndexHtml(HttpResponse response)
        {
            response.ContentType = "text/html;charset=utf-8";

            using (var stream = _options.IndexStream())
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

        private void RespondWithRedirect(HttpResponse response, string location)
        {
             response.StatusCode = 301;
             response.Headers["Location"] = location;
        }

        private StaticFileMiddleware CreateStaticFileMiddleware(
            RequestDelegate next,
            IWebHostEnvironment webHostEnv,
            ILoggerFactory loggerFactory,
            DataGeniesOptions options
            )
        {
            var staticFileOptions = new StaticFileOptions
            {
                RequestPath = string.IsNullOrEmpty(_options.RoutePrefix) ? string.Empty : $"/{options.RoutePrefix}",
                FileProvider = new EmbeddedFileProvider(typeof(DataGeniesMiddleware).GetTypeInfo().Assembly, EmbeddedFileNamespace)
            };
            
            return new StaticFileMiddleware(next, webHostEnv, Options.Create<StaticFileOptions>(staticFileOptions), loggerFactory);
        }
        
        private IDictionary<string, string> GetIndexArguments()
        {
            return new Dictionary<string, string>()
            {
                { "%(DocumentTitle)", _options.DocumentTitle },
            };
        }
    }
}