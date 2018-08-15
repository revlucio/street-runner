using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using Shouldly;
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
            content.ShouldNotBeEmpty();
        }

        [Fact]
        public async void ShouldReturnMapAsSvg()
        {
            var content = await GetContent("/api/map/east-london", "text/html");

            content.ShouldNotBeEmpty();

            var xml = XElement.Parse(content);
            var svg = xml.Element("body").Element("svg");
            svg.Elements("path").Count().ShouldBe(2431);
        }
        
        private static async Task<string> GetContent(string url, string contentType)
        {
            var response = await HttpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            response.Content.Headers.ContentType?.ToString().ShouldBe(contentType);

            return await response.Content.ReadAsStringAsync();
        }
    }
}