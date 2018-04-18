using System;
using System.Net.Http;

namespace StreetRunner.Web.Repositories
{
    public class StravaApiClient : IDisposable, IHttpClient
    {
        private readonly HttpClient _httpClient;

        public StravaApiClient()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://www.strava.com");
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer 93944545f252e152f5aeb0128fcca26760eadd01");
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