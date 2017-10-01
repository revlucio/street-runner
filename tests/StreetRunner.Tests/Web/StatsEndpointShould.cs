using System.IO;
using System.Linq;
using Web;
using Xunit;

namespace StreetRunner.Tests
{
    public class StatsEndpointShould
    {
        [Fact]
        public void OutputStreetTypeSummary()
        {
            var expected = 
@"0002 - primary
0001 - secondary
";

            var osm = @"
<osm>
 <way>
  <tag k=""highway"" v=""primary""/>
 </way>
  <way>
  <tag k=""highway"" v=""primary""/>
 </way>
 <way>
  <tag k=""highway"" v=""secondary""/>
 </way>
</osm>            
";

            var actual = new StatsEndpoint(osm).Get();

            Assert.Equal(expected, actual);
        }
    }
}
