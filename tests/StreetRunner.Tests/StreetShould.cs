using System.Collections.Generic;
using Xunit;

namespace StreetRunner.Tests
{
    public class StreetShould
    {
        [Fact]
        public void OutputToSvgPath() 
        {
            var street = new Street("name", new List<Point> 
            {
                { new Point(100m, 100m) },
                { new Point(200m, 200m) },
            });
            var svg = street.ToSvgPath(400, 400);

            Assert.Equal("<path d=\"M 0 400 L 400 0 \" stroke=\"black\" fill=\"transparent\"/>", svg);
        }

        // [Fact]
        // public void OutputToSvgPathAndScale() 
        // {
        //     var street = new Street("name", new List<Point> 
        //     {
        //         { new Point(100m, 100m) },
        //         { new Point(200m, 200m) },
        //         { new Point(300m, 300m) },
        //         { new Point(400m, 400m) },
        //     });
        //     var svg = street.ToSvgPath(4000, 40);

        //     Assert.Equal("<path d=\"M 1000 10 L 2000 20 L 3000 30 L 4000 40 \" stroke=\"black\" fill=\"transparent\"/>", svg);
        // }
    }
}
