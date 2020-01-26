using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DataGenies.AspNetCore.DataGeniesCore.Middlewares
{
    public interface IDataGeniesMiddlewareResponder
    {
        bool CanExecute(string httpMethod, string path);

        Task Respond(HttpContext httpContext, string path);
    }
}