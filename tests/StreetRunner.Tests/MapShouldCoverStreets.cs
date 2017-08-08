using System.Collections.Generic;
using Xunit;
using System.Linq;

namespace StreetRunner.Tests
{
    public class MapShouldCoverStreets
    {
        [Fact]
        public void NoRuns_IsNotCovered() 
        {
            var street = new Street("name", new List<Point> 
            {
                { new Point(0m, 0m) },
                { new Point(100m, 100m) },
            });
            var map = new Map(new List<Street>{street});

            Assert.False(map.Streets.Single().Covered);
        }

        [Fact]
        public void RunDoesNotCoverStreetAtAll_IsNotCovered() 
        {
            var street = new Street("name", new List<Point> 
            {
                { new Point(0m, 0m) },
                { new Point(100m, 100m) },
            });
            var map = new Map(new List<Street>{street});
            map.AddRun(@"
<gpx xmlns=""http://www.topografix.com/GPX/1/1""> 
<trk>
    <name>Test Run</name>
    <trkseg>
      <trkpt lat=""200"" lon=""200""></trkpt>
      <trkpt lat=""300"" lon=""300""></trkpt>
    </trkseg>
</trk>
</gpx>");

            Assert.False(map.Streets.Single().Covered);
        }

        [Fact]
        public void RunCoversStreetPerfectly_IsCovered() 
        {
            var street = new Street("name", new List<Point> 
            {
                { new Point(0m, 0m) },
                { new Point(100m, 100m) },
            });
            var map = new Map(new List<Street>{street});
            map.AddRun(@"
<gpx xmlns=""http://www.topografix.com/GPX/1/1""> 
<trk>
    <name>Test Run</name>
    <trkseg>
      <trkpt lat=""0"" lon=""0""></trkpt>
      <trkpt lat=""100"" lon=""100""></trkpt>
    </trkseg>
</trk>
</gpx>");

            Assert.True(map.Streets.Single().Covered);
        }
    }
}
