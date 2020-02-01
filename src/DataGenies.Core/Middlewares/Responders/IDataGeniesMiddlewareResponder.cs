using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DataGenies.Core.Middlewares.Responders
{
    public interface IDataGeniesMiddlewareResponder
    {
        bool CanExecute(string httpMethod, string path);

        Task Respond(HttpContext httpContext, string path);
    }
}