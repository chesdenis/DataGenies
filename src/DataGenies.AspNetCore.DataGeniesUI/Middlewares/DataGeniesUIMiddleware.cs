using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DataGenies.AspNetCore.DataGeniesCore.Middlewares;
using DataGenies.AspNetCore.DataGeniesCore.Middlewares.Responders;
using DataGenies.AspNetCore.DataGeniesCore.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DataGenies.AspNetCore.DataGeniesUI.Middlewares
{
    public class DataGeniesUIMiddleware
    {
        private readonly IEnumerable<IDataGeniesMiddlewareResponder> _responders;

        private const string EmbeddedFileNamespace =
            "DataGenies.AspNetCore.DataGeniesUI.ClientApp.dist";

        private readonly DataGeniesOptions _options;
        private readonly StaticFileMiddleware _staticFileMiddleware;

        public DataGeniesUIMiddleware(
            RequestDelegate next,
            IWebHostEnvironment webHostEnv,
            ILoggerFactory loggerFactory,
            DataGeniesOptions options,
            IEnumerable<IDataGeniesMiddlewareResponder> responders)
        {
            _responders = responders;
            _options = options ?? new DataGeniesOptions();
            _staticFileMiddleware = CreateStaticFileMiddleware(next,webHostEnv,loggerFactory, options);
        }
        
        public async Task Invoke(HttpContext httpContext)
        {
            var httpMethod = httpContext.Request.Method;
            var path = httpContext.Request.Path.Value;

            var responder = _responders.FirstOrDefault(f => f.CanExecute(httpMethod, path));
            if (responder != null)
            {
                await responder.Respond(httpContext, path);
            }

            await _staticFileMiddleware.Invoke(httpContext);
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
    }
}