using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xunit;

namespace E2ETests
{
    public class ApiRoot
    {
        private static readonly HttpClient HttpClient = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:5000")
        };

        [Fact]
        public async void ShouldReturnEndpoints()
        {
            var content = await GetContent("/api", "application/json");

            Assert.NotEmpty(content);
        }

        [Fact]
        public async void ShouldReturnMapAsSvg()
        {
            var content = await GetContent("/api/map/east-london", "text/html");

            Assert.NotEmpty(content);

            var xml = XElement.Parse(content);
            var svg = xml.Element("body").Element("svg");
            Assert.Equal(2431, svg.Elements("path").Count());
        }
        
        private static async Task<string> GetContent(string url, string contentType)
        {
            var response = await HttpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            Assert.Equal(contentType, response.Content.Headers.ContentType?.ToString());

            return await response.Content.ReadAsStringAsync();
        }
    }
}