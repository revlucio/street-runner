using System.Collections.Generic;
using System.Linq;
using StreetRunner.Core.Mapping;
using Xunit;

namespace StreetRunner.UnitTests.Domain
{
    public class StravaJsonRunShould
    {
        [Fact]
        public void HaveDefaultName()
        {
            var json = @"
[
    {
        ""data"": []
    }
]";
            IRun stravaJsonRun = new StravaJsonRun(json);

            Assert.Equal("Strava JSON Run", stravaJsonRun.Name);
        }

        [Fact]
        public void HavePoints()
        {
            var json = @"
[
    {
        ""data"": [
            [ 11.1, 22.2 ],
            [ 33.3, 44.4 ]
        ]
    }
]";
            IRun stravaJsonRun = new StravaJsonRun(json);

            Assert.Equal(11.1m, stravaJsonRun.Points.First().Lat);
            Assert.Equal(22.2m, stravaJsonRun.Points.First().Lon);
            Assert.Equal(33.3m, stravaJsonRun.Points.ElementAt(1).Lat);
            Assert.Equal(44.4m, stravaJsonRun.Points.ElementAt(1).Lon);
        }
    }
}