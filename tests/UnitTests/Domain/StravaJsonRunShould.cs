using System;
using System.Linq;
using Shouldly;
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
            parser.IsValid().ShouldBeTrue();

            var stravaJsonRun = parser.Value;
            stravaJsonRun.Points.First().Lat.ShouldBe(11.1m);
            stravaJsonRun.Points.First().Lon.ShouldBe(22.2m);
            stravaJsonRun.Points.ElementAt(1).Lat.ShouldBe(33.3m);
            stravaJsonRun.Points.ElementAt(1).Lon.ShouldBe(44.4m);
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
            parser.IsValid().ShouldBeTrue();

            var stravaJsonRun = parser.Value;

            stravaJsonRun.Id.ShouldBe("id");
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
            parser.IsValid().ShouldBeFalse();
            var ex = Assert.Throws<ArgumentException>(() => parser.Value);
            ex.Message.ShouldBe("JSON could not be parsed into a StravaJsonRun");
        }
    }
}