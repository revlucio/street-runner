using Newtonsoft.Json.Linq;
using Shouldly;
using StreetRunner.Core.Mapping;
using StreetRunner.Web.Endpoints;
using Xunit;

namespace StreetRunner.UnitTests.Web
{
    public class StreetsEndpointShould
    {
        [Fact]
        public void OutputStreeNames()
        {
            var map = new StubMap();
            map.AddStreet("Main Street");
            map.AddStreet("Wall Street");

            var streetsJson = new StreetsEndpoint(map).Get();
            var actual = JObject.Parse(streetsJson);
            
                actual.Value<JArray>("streets")[0].ShouldBe("Main Street");
                actual.Value<JArray>("streets")[1].ShouldBe("Wall Street");
        }
        
        [Fact]
        public void OutputEmptyListWhenThereAreNoStreets()
        {
            var map = new StubMap();

            var streetsJson = new StreetsEndpoint(map).Get();
            var actual = JObject.Parse(streetsJson);
            
            actual.Value<JArray>("streets").ShouldBeEmpty();
        }
    }
}
