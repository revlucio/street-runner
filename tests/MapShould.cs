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
    }
}
