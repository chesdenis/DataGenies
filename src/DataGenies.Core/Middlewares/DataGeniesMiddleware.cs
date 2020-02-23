using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataGenies.Core.Middlewares.Responders;
using DataGenies.Core.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace DataGenies.Core.Middlewares
{
    public class DataGeniesMiddleware
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly DataGeniesOptions _options;
        private readonly IEnumerable<IDataGeniesMiddlewareResponder> _responders;

        public DataGeniesMiddleware(
            RequestDelegate next,
            IWebHostEnvironment webHostEnv,
            ILoggerFactory loggerFactory,
            DataGeniesOptions options,
            IEnumerable<IDataGeniesMiddlewareResponder> responders)
        {
            this._options = options ?? new DataGeniesOptions();
            this._loggerFactory = loggerFactory;
            this._responders = responders;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var httpMethod = httpContext.Request.Method;
            var path = httpContext.Request.Path.Value;

            var responder = this._responders.FirstOrDefault(f => f.CanExecute(httpMethod, path));
            if (responder == null)
            {
                // var response = httpContext.Response;
                //response.ContentType = "text/html;charset=utf-8";
                //await response.WriteAsync("Incorrect url", Encoding.UTF8);
                return;
            }

            await responder.Respond(httpContext, path);
        }
    }
}