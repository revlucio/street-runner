using System;
using System.Linq;
using StreetRunner.Core.Mapping;
using Xunit;

namespace StreetRunner.UnitTests.Domain
{
    public class StravaJsonRunShould
    {
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

            var parser = new StravaRunParser(json, "id");
            Assert.Equal(true, parser.IsValid());

            var stravaJsonRun = parser.Value;
            Assert.Equal(11.1m, stravaJsonRun.Points.First().Lat);
            Assert.Equal(22.2m, stravaJsonRun.Points.First().Lon);
            Assert.Equal(33.3m, stravaJsonRun.Points.ElementAt(1).Lat);
            Assert.Equal(44.4m, stravaJsonRun.Points.ElementAt(1).Lon);
        }

        [Fact]
        public void HaveId()
        {
            var json = @"
[
    {
        ""data"": []
    }
]";
            var parser = new StravaRunParser(json, "id");
            Assert.Equal(true, parser.IsValid());

            var stravaJsonRun = parser.Value;

            Assert.Equal("id", stravaJsonRun.Id);
        }
        
        [Fact]
        public void BeInvalidIsJsonIsNotCorrect()
        {
            var json = @"
[
    {
    }
]";
            var parser = new StravaRunParser(json, "id");
            Assert.Equal(false, parser.IsValid());
            var ex = Assert.Throws<ArgumentException>(() => parser.Value);
            Assert.Equal("JSON could not be parsed into a StravaJsonRun", ex.Message);
        }
    }
}