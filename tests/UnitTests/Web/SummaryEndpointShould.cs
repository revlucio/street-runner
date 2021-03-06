using Newtonsoft.Json.Linq;
using Shouldly;
using StreetRunner.Web;
using StreetRunner.Web.Endpoints;
using Xunit;

namespace StreetRunner.UnitTests.Web
{
    public class SummaryEndpointShould
    {
        [Fact]
        public void ReturnNumberOfStreets()
        {
            var stubMap = new StubMap();
            stubMap.AddStreet();
            stubMap.AddStreet();
            stubMap.AddStreet();
            
            var summaryJson = new SummaryEndpoint(stubMap).Get();

            var summary = JObject.Parse(summaryJson);
            summary["totalNumberOfStreets"].ShouldBe(3);
        }
        
        [Fact]
        public void ReturnNumberOfStreetsCovered()
        {
            var stubMap = new StubMap();
            stubMap.AddStreet();
            stubMap.AddStreet();
            stubMap.AddCoveredStreet();
            
            var summaryJson = new SummaryEndpoint(stubMap).Get();

            var summary = JObject.Parse(summaryJson);
            summary["numberOfCoveredStreets"].ShouldBe(1);
        }
        
        [Fact]
        public void ReturnPercentageOfStreetsCovered()
        {
            var stubMap = new StubMap();
            stubMap.AddStreet();
            stubMap.AddCoveredStreet();
            
            var summaryJson = new SummaryEndpoint(stubMap).Get();

            var summary = JObject.Parse(summaryJson);
            summary["coveredPercentage"].ShouldBe(50m);
        }
        
        [Fact]
        public void ReturnTotalLengthCovered()
        {
            var stubMap = new StubMap();
            stubMap.AddStreet(length: 20m);
            stubMap.AddStreet(length: 10m);
            stubMap.AddStreet(length: 20m, covered: true);
            
            var summaryJson = new SummaryEndpoint(stubMap).Get();

            var summary = JObject.Parse(summaryJson);
            summary.Value<decimal>("totalLengthInMetres").ShouldBe(50);
            summary.Value<decimal>("totalLengthCoveredInMetres").ShouldBe(20);
            summary.Value<decimal>("distanceCoveredPercentage").ShouldBe(40);
        }
    }
}