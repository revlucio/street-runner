using System.Linq;
using Xunit;

namespace tests
{
    public class MapShould
    {
        [Fact]
        public void ImportStreetFromOdf()
        {
            var map = Map.FromOsd(@"
<osm>
 <way>
  <tag k=""name"" v=""Main Street""/>
 </way>
</osm>");

            Assert.Equal("Main Street", map.Streets.First().Name);
        }

        [Fact]
        public void ImportManyStreetsFromOdf()
        {
            var map = Map.FromOsd(@"
<osm>
 <way><tag k=""name"" v=""Main Street""/></way>
 <way><tag k=""name"" v=""Main Street""/></way>
 <way><tag k=""name"" v=""Main Street""/></way>
</osm>");

            Assert.Equal(3, map.Streets.Count());
        }

        [Fact]
        public void ImportStartAndEndNodes()
        {
            var map = Map.FromOsd(@"
<osm>
 <node id=""111"" lat=""11.1234567"" lon=""22.1234567""/>
 <node id=""222"" lat=""33.1234567"" lon=""44.1234567""/>
 <way>
  <nd ref=""111""/>
  <nd ref=""222""/>
  <tag k=""name"" v=""Main Street""/>
 </way>
</osm>");

            var street = map.Streets.First();
            Assert.Equal("Main Street", street.Name);
            Assert.Equal(11.1234567m, street.Points.First().Lat);
            Assert.Equal(22.1234567m, street.Points.First().Lon);
            Assert.Equal(33.1234567m, street.Points.Last().Lat);
            Assert.Equal(44.1234567m, street.Points.Last().Lon);
        }
    }
}
