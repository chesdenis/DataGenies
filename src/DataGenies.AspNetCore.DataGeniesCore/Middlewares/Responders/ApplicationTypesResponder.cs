using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DataGenies.AspNetCore.DataGeniesCore.Models;
using DataGenies.AspNetCore.DataGeniesCore.Scanners;
using Microsoft.AspNetCore.Http;

namespace DataGenies.AspNetCore.DataGeniesCore.Middlewares.Responders
{
    public class ApplicationTypesResponder : IDataGeniesMiddlewareResponder
    {
        private readonly DataGeniesOptions _options;
        private readonly IApplicationTypesScanner _applicationTypesScanner;

        public ApplicationTypesResponder(DataGeniesOptions options, IApplicationTypesScanner applicationTypesScanner)
        {
            _options = options;
            _applicationTypesScanner = applicationTypesScanner;
        }
        
        public bool CanExecute(string httpMethod, string path)
        {
            return httpMethod == "GET" && Regex.IsMatch(path, $"^/{_options.RoutePrefix}/api/application-types");
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
     
    // public class ApplicationInstancesResponder : IDataGeniesMiddlewareResponder
    // {
    //     private readonly DataGeniesOptions _options;
    //    
    //
    //     public ApplicationTypesResponder(DataGeniesOptions options, IDataGeniesRepository dataGeniesRepository)
    //     {
    //         _options = options;
    //         
    //     }
    //     
    //     public bool CanExecute(string httpMethod, string path)
    //     {
    //         return httpMethod == "GET" && Regex.IsMatch(path, $"^/{_options.RoutePrefix}/api/application-types");
    //     }
    //
    //     public async Task Respond(HttpContext httpContext, string path)
    //     {
    //         var response = httpContext.Response;
    //         
    //         response.ContentType = "application/json;charset=utf-8";
    //
    //         var applicationTypes = _applicationTypesScanner.ScanTypes();
    //         var responseData = JsonSerializer.Serialize(applicationTypes);
    //
    //         await response.WriteAsync(responseData, Encoding.UTF8);
    //     }
    // }
}