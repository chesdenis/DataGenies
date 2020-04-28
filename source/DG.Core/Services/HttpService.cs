using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DG.Core.Services
{
    public class HttpService : IHttpService
    {
        private AuthenticationHeaderValue authenticationHeaderValue;

        public IHttpService WithBasicAuthHeader(string headerContent)
        {
            var credentials = Encoding.ASCII.GetBytes(headerContent);
            this.authenticationHeaderValue = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(credentials));
            return this;
        }

        public async Task<string> Get(string url)
        {
            using var client = new HttpClient();

            if (this.authenticationHeaderValue != null)
            {
                client.DefaultRequestHeaders.Authorization = this.authenticationHeaderValue;
            }

            using var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> Post(string url, string content, string mediaType = "application/json")
        {
            using var client = new HttpClient();

            if (this.authenticationHeaderValue != null)
            {
                client.DefaultRequestHeaders.Authorization = this.authenticationHeaderValue;
            }

            var stringContent = new StringContent(content);
            stringContent.Headers.ContentType = new MediaTypeHeaderValue(mediaType);
            using var response = await client.PostAsync(url, stringContent);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> Put(string url, string content, string mediaType = "application/json")
        {
            using var client = new HttpClient();

            if (this.authenticationHeaderValue != null)
            {
                client.DefaultRequestHeaders.Authorization = this.authenticationHeaderValue;
            }

            var stringContent = new StringContent(content);
            stringContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            
            using var response = await client.PutAsync(url, stringContent);
            response.EnsureSuccessStatusCode();
            
            stringContent.Dispose();

            return await response.Content.ReadAsStringAsync();
        }
    }
}