using System;
using System.Linq;
using System.Net;
using AngleSharp.Extensions;
using AngleSharp.Parser.Html;
using Microsoft.AspNetCore.Mvc.Testing;
using Shouldly;
using StreetRunner.Web;
using Xunit;

namespace StreetRunner.UnitTests.Web
{
    public class StartupShould : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public StartupShould(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }
        
        [Fact(Skip = "needs a lot of setup first")]
        public async void Load()
        {
            // had to handle content root somehow
            // have to manually load in east-london map through fs instead of api
            // load this from file or stub it out
            // need to stub out strava API
            Environment.SetEnvironmentVariable("STRAVA_SECRET", "99b638c3a276b3ad3813dd5c8a667b3d670ea555");
            
            var client = _factory.CreateClient();
            
            var response = await client.GetAsync("/api/map/east-london/strava");
            var content = await response.Content.ReadAsStringAsync();

            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                var parser = new HtmlParser();
                var document = parser.Parse(content);
                var error = document.GetElementsByClassName("stackerror").Single();
                
                response.StatusCode.ShouldNotBe(HttpStatusCode.InternalServerError, error.TextContent);
            }
            
//            response.StatusCode.ShouldNotBe(HttpStatusCode.NotFound, response.RequestMessage.RequestUri.ToString());
//            response.EnsureSuccessStatusCode();
        }
    }
}