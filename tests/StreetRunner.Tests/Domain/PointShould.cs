using StreetRunner.Mapping;
using Xunit;

namespace tests.Domain
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
            var distance = new Point(50, 50).CalculateDistanceInMetres(new Point(50.001m, 50.001m));

            Assert.Equal(132, distance);
        }
    }
}
