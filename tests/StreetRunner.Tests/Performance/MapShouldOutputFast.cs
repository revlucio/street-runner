using System.Diagnostics;
using System.IO;
using StreetRunner.Mapping;
using Xunit;

namespace tests.Performance
{
    public class MapShouldOutputFast
    {
        [Fact]
        public void OutputSmallFile() 
        {
            var osm = File.ReadAllText($"{Settings.FilesDirectory()}/small-east-london.osm");

            var timer = Stopwatch.StartNew();
            var svg = Map.FromOsd(osm).ToSvgPath(500, 500);
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
            var map = Map.FromOsd(osm);
            map.AddRun(gpx);
            var svg = map.ToSvgPath(500, 500);
            timer.Stop();
            
            Assert.InRange(timer.ElapsedMilliseconds, 0, 400);
        }
    }
}
