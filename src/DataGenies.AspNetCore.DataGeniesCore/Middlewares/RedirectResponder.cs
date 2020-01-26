using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DataGenies.AspNetCore.DataGeniesCore.Models;
using Microsoft.AspNetCore.Http;

namespace DataGenies.AspNetCore.DataGeniesCore.Middlewares
{
    public class RedirectResponder : IDataGeniesMiddlewareResponder
    {
        private readonly DataGeniesOptions _options;

        public RedirectResponder(DataGeniesOptions options)
        {
            _options = options;
        }
        
        public bool CanExecute(string httpMethod, string path)
        {
            return httpMethod == "GET" && Regex.IsMatch(path, $"^/{_options.RoutePrefix}/?$");
        }

        public async Task Respond(HttpContext httpContext, string path)
        {
            // Use relative redirect to support proxy environments
            var response = httpContext.Response;
            var relativeRedirectPath = path.EndsWith("/")
                ? "index.html"
                : $"{path.Split('/').Last()}/index.html";
            response.StatusCode = 301;
            response.Headers["Location"] = relativeRedirectPath;
        }
    }
}