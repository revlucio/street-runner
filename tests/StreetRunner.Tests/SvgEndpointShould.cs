using System.IO;
using System.Linq;
using Web;
using Xunit;

namespace StreetRunner.Tests
{
    public class SvgEndpointShould
    {
        [Fact]
        public void OutputSvg()
        {
            var expected = 
@"<html>
<body>
<svg width=""500"" height=""500"">
<path d=""M 0 500 L 500 0.00000000000000000000000001 "" stroke=""black"" fill=""transparent""/>
</svg>
</body>
</html>
";

            var osm = @"
<osm>
 <node id=""111"" lat=""11.1"" lon=""22.2""/>
 <node id=""222"" lat=""33.3"" lon=""44.4""/>
 <way>
  <nd ref=""111""/>
  <nd ref=""222""/>
  <tag k=""highway"" v=""secondary""/>
 </way>
</osm>            
";

            var actual = new SvgEndpoint(osm).Get();

            Assert.Equal(expected, actual);
        }
    }
}
