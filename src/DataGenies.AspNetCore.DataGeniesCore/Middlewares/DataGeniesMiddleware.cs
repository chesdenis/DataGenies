using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataGenies.AspNetCore.DataGeniesCore.Middlewares.Responders;
using Microsoft.AspNetCore.Http;

namespace DataGenies.AspNetCore.DataGeniesCore.Middlewares
{
    public class DataGeniesMiddleware
    {
        private readonly IEnumerable<IDataGeniesMiddlewareResponder> _responders;

        public DataGeniesMiddleware(IEnumerable<IDataGeniesMiddlewareResponder> responders)
        {
            _responders = responders;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var httpMethod = httpContext.Request.Method;
            var path = httpContext.Request.Path.Value;

            var responder = _responders.First(f => f.CanExecute(httpMethod, path));

            await responder.Respond(httpContext, path);
        }
    }
}