using System.Collections.Generic;
using StreetRunner.Core.Mapping;
using StreetRunner.Web.Endpoints;
using StreetRunner.Web.Repositories;
using Xunit;

namespace StreetRunner.UnitTests.Web
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
<path d=""M 0 500 L 500 0 "" stroke=""yellow"" fill=""transparent""/>
<path d=""M 0 500 L 500 0 "" stroke=""red"" fill=""transparent""/>
</svg>
</body>
</html>
";

            var osm = @"
<osm>
 <node id=""1"" lat=""10"" lon=""10""/>
 <node id=""2"" lat=""20"" lon=""20""/>
 <way>
  <nd ref=""1""/>
  <nd ref=""2""/>
  <tag k=""highway"" v=""secondary""/>
 </way>
</osm>            
";

            var gpx = @"<gpx>
<metadata>
  <time>2018-04-16T15:05:30Z</time>
</metadata>
<trk>
    <name>Test Run</name>
    <trkseg>
      <trkpt lat=""10"" lon=""10""></trkpt>
      <trkpt lat=""20"" lon=""20""></trkpt>
    </trkseg>
</trk>
</gpx>";
            
            var stubFinder = new StubMapFinder(
                new Dictionary<string, string>{ { "mapName", osm }}, 
                new List<string> { gpx });
            var actual = new SvgEndpoint("mapName", new FileSystemMapRepository(
                stubFinder, 
                new FileSystemRunRepository(stubFinder),
                new CoveredStreetCalculator())).Get();

            Assert.Equal(expected, actual);
        }
    }
}
