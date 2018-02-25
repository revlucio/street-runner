using System;
using System.Net.Http;
using StreetRunner.Web.Endpoints;

namespace StreetRunner.Web
{
    public class RestHttpClient : IHttpClient
    {
        public string Get(string url)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer 93944545f252e152f5aeb0128fcca26760eadd01");

            try
            {
                var response = client.GetAsync(url, HttpCompletionOption.ResponseContentRead);
                var httpResponseMessage = response.Result;
                return httpResponseMessage.Content.ReadAsStringAsync().Result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return "";
            }
        }
    }
}