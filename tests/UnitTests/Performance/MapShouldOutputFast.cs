using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using StreetRunner.Core.Mapping;
using Xunit;

namespace StreetRunner.UnitTests.Performance
{
    public class MapShouldOutputFast
    {
        [Fact]
        public void OutputSmallFile() 
        {
            var osm = File.ReadAllText($"{Settings.FilesDirectory()}/small-east-london.osm");

            var timer = Stopwatch.StartNew();
            var svg = MapFactory.FromOsm(osm).ToSvg(500, 500);
            timer.Stop();
            
            Assert.InRange(timer.ElapsedMilliseconds, 0, 100);
        }

        [Fact]
        public void OutputSmallFileWithRun() 
        {
            var filesDir = Settings.FilesDirectory();
            var osm = File.ReadAllText($"{filesDir}/small-east-london.osm");
            var gpx = File.ReadAllText($"{filesDir}/east-london-run.gpx");

            var timer = Stopwatch.StartNew();
            
            var run = Run.FromGpx(gpx);
            var map = MapFactory.FromOsm(osm, new List<IRun>{run});
            
            map.ToSvg(500, 500);
            
            timer.Stop();
            
            Assert.InRange(timer.ElapsedMilliseconds, 0, 400);
        }
    }
}
