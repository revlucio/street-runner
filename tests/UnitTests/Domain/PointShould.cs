using Shouldly;
using StreetRunner.Core.Mapping;
using Xunit;

namespace StreetRunner.UnitTests.Domain
{
    public class PointShould
    {
        [Fact]
        public void Return0MetresIfPointsAreTheSame() 
        {
            var distance = new Point(100, 100).CalculateDistanceInMetres(new Point(100, 100));
            distance.ShouldBe(0);
        }

        [Fact]
        public void ReturnCorrectDistanceIfTheyAreNotTheSame() 
        {
            var distance = new Point(50, 50).CalculateDistanceInMetres(new Point(50.001m, 50.001m));
            distance.ShouldBe(132);
        }
    }
}
