using System.Linq;
using Shouldly;
using StreetRunner.Core.Mapping;
using Xunit;

namespace StreetRunner.UnitTests.Domain
{
    public class MapShouldImport
    {
        [Fact]
        public void ImportStreetFromOdf()
        {
            var map = MapFactory.FromOsm(@"
<osm>
 <way>
  <tag k=""highway"" v=""secondary"" />
 </way>
</osm>");

            map.Streets.Count().ShouldBe(1);
        }

        [Fact]
        public void ImportStreetNameFromOdf()
        {
            var map = MapFactory.FromOsm(@"
<osm>
 <way>
  <tag k=""name"" v=""Main Street""/>
  <tag k=""highway"" v=""secondary"" />
 </way>
</osm>");

            map.Streets.First().Name.ShouldBe("Main Street");
        }

        [Fact]
        public void ImportOnlyHighways()
        {
            var map = MapFactory.FromOsm(@"
<osm>
 <way>
  <tag k=""building"" v=""yes"" />
 </way>
</osm>");

            map.Streets.ShouldBeEmpty();
        }

        [Fact]
        public void ImportManyStreetsFromOdf()
        {
            var map = MapFactory.FromOsm(@"
<osm>
 <way><tag k=""highway"" v=""secondary""/></way>
 <way><tag k=""highway"" v=""secondary""/></way>
 <way><tag k=""highway"" v=""secondary""/></way>
</osm>");

            map.Streets.Count().ShouldBe(3);
        }

        [Fact]
        public void ImportStartAndEndNodes()
        {
            var map = MapFactory.FromOsm(@"
<osm>
 <node id=""111"" lat=""11.1"" lon=""22.2""/>
 <node id=""222"" lat=""33.3"" lon=""44.4""/>
 <way>
  <nd ref=""111""/>
  <nd ref=""222""/>
  <tag k=""highway"" v=""secondary""/>
 </way>
</osm>");

            var street = map.Streets.First();
            street.Points.First().Lat.ShouldBe(11.1m);
            street.Points.First().Lon.ShouldBe(22.2m);
            street.Points.Last().Lat.ShouldBe(33.3m);
            street.Points.Last().Lon.ShouldBe(44.4m);
        }
    }
}
