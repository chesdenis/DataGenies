using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DataGenies.AspNetCore.DataGeniesCore.Models;
using DataGenies.AspNetCore.DataGeniesCore.Scanners;
using Microsoft.AspNetCore.Http;

namespace DataGenies.AspNetCore.DataGeniesCore.Middlewares.Responders
{
    public class GetApplicationTypesResponder : IDataGeniesMiddlewareResponder
    {
        private readonly DataGeniesOptions _options;
        private readonly IApplicationTypesScanner _applicationTypesScanner;

        public GetApplicationTypesResponder(DataGeniesOptions options, IApplicationTypesScanner applicationTypesScanner)
        {
            _options = options;
            _applicationTypesScanner = applicationTypesScanner;
        }
        
        public bool CanExecute(string httpMethod, string path)
        {
            return httpMethod == "GET" && Regex.IsMatch(path, $"^/{_options.RoutePrefix}/ApplicationTypes");
        }

        public async Task Respond(HttpContext httpContext, string path)
        {
            var response = httpContext.Response;
            
            response.ContentType = "application/json;charset=utf-8";

            var applicationTypes = _applicationTypesScanner.ScanTypes();
            var responseData = JsonSerializer.Serialize(applicationTypes);

            await response.WriteAsync(responseData, Encoding.UTF8);
        }
    }
}