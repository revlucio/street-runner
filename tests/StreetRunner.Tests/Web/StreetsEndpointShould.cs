using System.IO;
using System.Linq;
using Web;
using Xunit;

namespace StreetRunner.Tests
{
    public class StreetsEndpointShould
    {
        [Fact]
        public void OutputListOfStreets()
        {
            var expected = 
@"  Main Street
  Wall Street
";

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

            var actual = new StreetsEndpoint(osm).Get();

            Assert.Equal(expected, actual);
        }
    }
}
