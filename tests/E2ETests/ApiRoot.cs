using System;
using System.Net;
using System.Net.Http;
using Xunit;

namespace E2ETests
{
    public class ApiRoot
    {
        [Fact]
        public async void ShouldReturnEndpoints()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:5000");

            var response = await client.GetAsync("/api");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            
            Assert.Equal("application/json", response.Content.Headers.ContentType.ToString());
            Assert.NotEmpty(content);
        }
    }
}