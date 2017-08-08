using System.Collections.Generic;
using Xunit;
using System.Linq;

namespace StreetRunner.Tests
{
    public class PointShould
    {
        [Fact]
        public void Return0MetresIfPointsAreTheSame() 
        {
            var distance = new Point(100, 100).CalculateDistanceInMetres(new Point(100, 100));

            Assert.Equal(0m, distance);
        }

        [Fact]
        public void ReturnCorrectDistanceIfTheyAreNotTheSame() 
        {
            var distance = new Point(50, 50).CalculateDistanceInMetres(new Point(50.1m, 50.1m));

            Assert.Equal(13229, distance);
        }
    }
}
