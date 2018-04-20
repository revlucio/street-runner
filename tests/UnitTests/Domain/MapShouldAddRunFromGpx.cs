using System.Collections.Generic;
using System.Linq;
using StreetRunner.Core.Mapping;
using Xunit;

namespace StreetRunner.UnitTests.Domain
{
    public class MapShouldAddRunFromGpx
    {
        [Fact]
        public void WithId()
        {
            var run = Run.FromGpx(@"
<gpx xmlns=""http://www.topografix.com/GPX/1/1"">
<metadata>
  <time>2018-04-16T15:05:30Z</time>
</metadata>
<trk>
    <name>Test Run</name>
</trk>
</gpx>");
            var map = new Map(Enumerable.Empty<Street>(), new List<IRun> { run });

            Assert.Equal("2018-04-16T15:05:30Z", map.Runs.First().Id);
        }

        [Fact]
        public void WithPoint()
        {
            var run = Run.FromGpx(@"
<gpx xmlns=""http://www.topografix.com/GPX/1/1""> 
<metadata>
  <time>2018-04-16T15:05:30Z</time>
</metadata>
<trk>
    <name>Test Run</name>
    <trkseg>
      <trkpt lat=""11.1"" lon=""22.2""></trkpt>
      <trkpt lat=""33.3"" lon=""44.4""></trkpt>
    </trkseg>
</trk>
</gpx>");
            var map = new Map(Enumerable.Empty<Street>(), new List<IRun> { run });

            Assert.Equal(11.1m, map.Runs.First().Points.First().Lat);
            Assert.Equal(22.2m, map.Runs.First().Points.First().Lon);
            Assert.Equal(33.3m, map.Runs.First().Points.ElementAt(1).Lat);
            Assert.Equal(44.4m, map.Runs.First().Points.ElementAt(1).Lon);
        }

        [Fact]
        public void ExportsRunInRed()
        {
            var run = Run.FromGpx(@"
<gpx>
<metadata>
  <time>2018-04-16T15:05:30Z</time>
</metadata>
<trk>
    <name>Test Run</name>
    <trkseg>
      <trkpt lat=""100"" lon=""100""></trkpt>
      <trkpt lat=""200"" lon=""200""></trkpt>
    </trkseg>
</trk>
</gpx>");
            var map = new Map(Enumerable.Empty<Street>(), new List<IRun> { run });

            var svg = map.ToSvg(100, 100);

            Assert.Equal("<path d=\"M 0 100 L 100 0 \" stroke=\"red\" fill=\"transparent\"/>", svg);
        }

        [Fact]
        public void CoveredStreetIsYellow()
        {
            var street = new Street("name", new List<Point> {
                new Point(100, 100),
                new Point(200, 200),
            });
            var run = new Run(new List<Point> {
                new Point(100, 100),
                new Point(200, 200),
            }, "time");
            var map = new Map(new List<Street> { street }, new [] {run});

            var svg = map.ToSvg(100, 100);

            Assert.Equal(
@"<path d=""M 0 100 L 100 0 "" stroke=""yellow"" fill=""transparent""/>
<path d=""M 0 100 L 100 0 "" stroke=""red"" fill=""transparent""/>", svg);
        }
    }
}
