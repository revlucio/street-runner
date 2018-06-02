using System.Linq;
using Newtonsoft.Json.Linq;
using StreetRunner.Core.Mapping;
using Xunit;

namespace StreetRunner.UnitTests.Domain
{
    public class MapShouldOutputCoveredStreets
    {
        [Fact]
        public void EmptyMapEmptyJson()
        {
            var map = new Map(Enumerable.Empty<Street>(), Enumerable.Empty<IRun>());

            var json = map.ToJson();
            
            Assert.Empty(json["runIds"]);
            Assert.Empty(json["coveredStreets"]);
        }
        
        [Fact]
        public void NotHaveStreetsThatAreNotCovered()
        {
            var street = new Street("Main St", new [] {new Point(0,0), new Point(10,01)});
            var run = new Run(new [] {new Point(100,1000), new Point(200,200)}, "time");
            var map = new Map(new[] {street}, new[] {run});

            var json = map.ToJson();

            Assert.Equal("time", json.Value<JArray>("runIds").Single());
            Assert.Empty(json["coveredStreets"]);
        }
        
        [Fact]
        public void HaveStreetsThatAreCovered()
        {
            var street = new Street("Main St", new [] {new Point(0,0), new Point(10,01)});
            var run = new Run(new [] {new Point(0,0), new Point(10,01)}, "time");
            var map = new Map(new[] {street}, new[] {run});

            var json = map.ToJson();

            Assert.Equal("time", json.Value<JArray>("runIds").Single());
            Assert.Equal("Main St", json.Value<JArray>("coveredStreets").Single());
        }
        
        [Fact]
        public void HandleEmptyJson()
        {
            var map = new Map(Enumerable.Empty<Street>(), Enumerable.Empty<IRun>());

            Assert.True(map != null);
        }
    }
}