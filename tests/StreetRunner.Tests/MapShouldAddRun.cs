using System.Linq;
using Xunit;

namespace StreetRunner.Tests
{
    public class MapShouldAddRun
    {
        [Fact]
        public void WithName()
        {
            var map = new Map(Enumerable.Empty<Street>());
            map.AddRun(@"
<gpx>
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
<gpx>
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

            Assert.Equal("<path d=\"M 100 0 L 0 100 \" stroke=\"red\" fill=\"transparent\"/>", svg);
        }
    }
}