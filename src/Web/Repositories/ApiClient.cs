using System;
using System.Net.Http;

namespace StreetRunner.Web.Repositories
{
    public class ApiClient : IDisposable, IHttpClient
    {
        private readonly HttpClient _httpClient;

        public ApiClient()
        {
            _httpClient = new HttpClient();
        }
        
        public string Get(string url)
        {
            var response = _httpClient.GetAsync(url).Result;
            var content = response.Content.ReadAsStringAsync().Result;
            return content;
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}