using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace StreetRunner.Tests
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
    }
}
