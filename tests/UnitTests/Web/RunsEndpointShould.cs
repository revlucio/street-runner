using System.Linq;
using Newtonsoft.Json.Linq;
using Shouldly;
using StreetRunner.Web.Endpoints;
using Xunit;

namespace StreetRunner.UnitTests.Web
{
    public class RunsEndpointShould
    {
        [Fact]
        public void ReturnListOfRuns()
        {
            var stubMap = new StubMap();
            stubMap.AddRunOutOfLondon();
            stubMap.AddRunOutOfLondon();
            
            var summaryJson = new RunsEndpoint(stubMap).Get();

            var summary = JObject.Parse(summaryJson);
            summary.Value<JArray>("runs").Count.ShouldBe(2);
        }
        
        [Fact]
        public void ReturnIfInLondon()
        {
            var stubMap = new StubMap();
            stubMap.AddRunInLondon();
            
            var runJson = GetSingleRunJsonFromRunsEndpoint(stubMap);

            runJson["isInLondon"].ShouldBe(true);
        }

        [Fact]
        public void ReturnIfOutOfLondon()
        {
            var stubMap = new StubMap();
            stubMap.AddRunOutOfLondon();
            
            var runJson = GetSingleRunJsonFromRunsEndpoint(stubMap);
            
            runJson["isInLondon"].ShouldBe(false);
        }
        
        private static JToken GetSingleRunJsonFromRunsEndpoint(StubMap stubMap)
        {
            var summaryJson = new RunsEndpoint(stubMap).Get();
            var summary = JObject.Parse(summaryJson);
            return summary.Value<JArray>("runs").Single();
        }
    }
}