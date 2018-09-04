using Newtonsoft.Json.Linq;
using Shouldly;
using StreetRunner.Web.Endpoints;
using Xunit;

namespace StreetRunner.UnitTests.Web
{
    public class StreetsEndpointShould
    {
        [Fact]
        public void OutputStreeNames()
        {
            var osm = @"
<osm>
 <way>
  <tag k=""highway"" v=""secondary""/>
  <tag k=""name"" v=""Main Street""/>
 </way>
 <way>
  <tag k=""highway"" v=""secondary""/>
  <tag k=""name"" v=""Wall Street""/>
 </way>
</osm>            
";

            var streetsJson = new StreetsEndpoint(osm).Get();
            var actual = JObject.Parse(streetsJson);
            
                actual.Value<JArray>("streets")[0].ShouldBe("Main Street");
                actual.Value<JArray>("streets")[1].ShouldBe("Wall Street");
        }
        
        [Fact]
        public void OutputEmptyListWhenThereAreNoStreets()
        {
            var osm = @"
<osm>
</osm>            
";

            var streetsJson = new StreetsEndpoint(osm).Get();
            var actual = JObject.Parse(streetsJson);
            
            actual.Value<JArray>("streets").ShouldBeEmpty();
        }
    }
}
