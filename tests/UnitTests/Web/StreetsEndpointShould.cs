using Newtonsoft.Json.Linq;
using Shouldly;
using StreetRunner.Web.Endpoints;
using Xunit;

namespace StreetRunner.UnitTests.Web
{
    public class StreetsEndpointShould
    {
        [Fact]
        public void OutputEmptyListWhenThereAreNoStreets()
        {
            var map = new StubMap();

            var streetsJson = new StreetsEndpoint(map).Get();
            var actual = JObject.Parse(streetsJson);
            
            actual.Value<JArray>("streets").ShouldBeEmpty();
        }
        
        [Fact]
        public void OutputStreetNames()
        {
            var map = new StubMap();
            map.AddStreet("Main Street");
            map.AddStreet("Wall Street");

            var streetsJson = new StreetsEndpoint(map).Get();
            var actual = JObject.Parse(streetsJson);
            
            actual.Value<JArray>("streets")[0]["name"].ShouldBe("Main Street");
            actual.Value<JArray>("streets")[1]["name"].ShouldBe("Wall Street");
        }
        
        [Fact]
        public void OutputCovered()
        {
            var map = new StubMap();
            map.AddCoveredStreet();
            map.AddStreet();

            var streetsJson = new StreetsEndpoint(map).Get();
            var actual = JObject.Parse(streetsJson);
            
            actual.Value<JArray>("streets")[0]["covered"].ShouldBe(true);
            actual.Value<JArray>("streets")[1]["covered"].ShouldBe(false);
        }
        
        [Fact]
        public void OutputLength()
        {
            var map = new StubMap();
            map.AddStreet(length: 50m);

            var streetsJson = new StreetsEndpoint(map).Get();
            var actual = JObject.Parse(streetsJson);
            
            actual.Value<JArray>("streets")[0].Value<decimal>("length").ShouldBe(50);
        }
    }
}
