using System.Text;
using System.Threading.Tasks;

namespace DG.Core.Services
{
    public interface IHttpService
    {
        IHttpService WithBasicAuthHeader(string headerContent);

        Task<string> Get(string url);

        Task<string> Post(string url, string content, string mediaType = "application/json");

        Task<string> Put(string url, string content, string mediaType = "application/json");
    }
}