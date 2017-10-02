using System.Collections.Generic;
using System.Linq;
using StreetRunner.Mapping;
using Xunit;

namespace tests
{
    public class MapShouldAddRun
    {
        [Fact]
        public void WithName()
        {
            var map = new Map(Enumerable.Empty<Street>());
            map.AddRun(@"
<gpx xmlns=""http://www.topografix.com/GPX/1/1"">
<trk>
    <name>Test Run</name>
</trk>
</gpx>");

            Assert.Equal("Test Run", map.Runs.First().Name);
        }

        [Fact]
        public void WithPoint()
        {
            var map = new Map(Enumerable.Empty<Street>());
            map.AddRun(@"
<gpx xmlns=""http://www.topografix.com/GPX/1/1""> 
<trk>
    <name>Test Run</name>
    <trkseg>
      <trkpt lat=""11.1"" lon=""22.2""></trkpt>
      <trkpt lat=""33.3"" lon=""44.4""></trkpt>
    </trkseg>
</trk>
</gpx>");

            Assert.Equal(11.1m, map.Runs.First().Points.First().Lat);
            Assert.Equal(22.2m, map.Runs.First().Points.First().Lon);
            Assert.Equal(33.3m, map.Runs.First().Points.ElementAt(1).Lat);
            Assert.Equal(44.4m, map.Runs.First().Points.ElementAt(1).Lon);
        }

        [Fact]
        public void ExportsRunInRed()
        {
            var map = new Map(Enumerable.Empty<Street>());
            map.AddRun(@"
<gpx>
<trk>
    <name>Test Run</name>
    <trkseg>
      <trkpt lat=""100"" lon=""100""></trkpt>
      <trkpt lat=""200"" lon=""200""></trkpt>
    </trkseg>
</trk>
</gpx>");

            var svg = map.ToSvgPath(100, 100);

            Assert.Equal("<path d=\"M 0 100 L 100 0 \" stroke=\"red\" fill=\"transparent\"/>", svg);
        }

        [Fact]
        public void CoveredStreetIsYellow()
        {
            var street = new Street("name", new List<Point> {
                new Point(100, 100),
                new Point(200, 200),
            });
            var map = new Map(new List<Street> { street });
            var run = new Run("name", new List<Point> {
                new Point(100, 100),
                new Point(200, 200),
            });
            map.AddRun(run);

            var svg = map.ToSvgPath(100, 100);

            Assert.Equal(
@"<path d=""M 0 100 L 100 0 "" stroke=""yellow"" fill=""transparent""/>
<path d=""M 0 100 L 100 0 "" stroke=""red"" fill=""transparent""/>", svg);
        }
    }
}
