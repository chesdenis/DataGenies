using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataGenies.AspNetCore.DataGeniesCore.Middlewares.Responders;
using DataGenies.AspNetCore.DataGeniesCore.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DataGenies.AspNetCore.DataGeniesCore.Middlewares
{
    public class DataGeniesMiddleware
    {
        private readonly DataGeniesOptions _options;
        private readonly ILoggerFactory _loggerFactory;
        private readonly IEnumerable<IDataGeniesMiddlewareResponder> _responders;

        public DataGeniesMiddleware(
            RequestDelegate next,
            IWebHostEnvironment webHostEnv,
            ILoggerFactory loggerFactory,
            DataGeniesOptions options,
            IEnumerable<IDataGeniesMiddlewareResponder> responders
            )
        {
            _options = options ?? new DataGeniesOptions();
            _loggerFactory = loggerFactory;
            _responders = responders;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var httpMethod = httpContext.Request.Method;
            var path = httpContext.Request.Path.Value;

            var responder = _responders.FirstOrDefault(f => f.CanExecute(httpMethod, path));
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