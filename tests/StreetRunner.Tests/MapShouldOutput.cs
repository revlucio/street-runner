using System.Collections.Generic;
using StreetRunner.Mapping;
using Xunit;

namespace tests
{
    public class MapShouldOutput
    {
        [Fact]
        public void OutputToSvgPath_InvertYAxis() 
        {
            var street = new Street("name", new List<Point> 
            {
                { new Point(0m, 0m) },
                { new Point(100m, 100m) },
            });
            var map = new Map(new List<Street>{street});
            var svg = map.ToSvgPath(100, 100);

            Assert.Equal("<path d=\"M 0 100 L 100 0 \" stroke=\"black\" fill=\"transparent\"/>", svg);
        }

        [Fact]
        public void OutputToSvgPath_AndScale() 
        {
            var street = new Street("name", new List<Point> 
            {
                { new Point(100m, 100m) },
                { new Point(200m, 200m) },
            });
            var map = new Map(new List<Street>{street});
            var svg = map.ToSvgPath(400, 400);

            Assert.Equal("<path d=\"M 0 400 L 400 0 \" stroke=\"black\" fill=\"transparent\"/>", svg);
        }

        [Fact]
        public void OutputMultipleStreetsToSvgPath_AndScale() 
        {
            var street1 = new Street("name", new List<Point> 
            {
                { new Point(100m, 100m) },
                { new Point(200m, 200m) },
            });
            var street2 = new Street("name", new List<Point> 
            {
                { new Point(200m, 200m) },
                { new Point(300m, 300m) },
            });
            var map = new Map(new List<Street>{street1, street2});
            var svg = map.ToSvgPath(400, 400);

            Assert.Equal(
@"<path d=""M 0 400 L 200 200 "" stroke=""black"" fill=""transparent""/>
<path d=""M 200 200 L 400 0 "" stroke=""black"" fill=""transparent""/>"
            , svg);
        }
    }
}
