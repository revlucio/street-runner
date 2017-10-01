using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Xunit;

namespace StreetRunner.Tests
{
    public class MapShouldOutputFast
    {
        [Fact(Skip = "still too slow")]
        public void OutputSmallFile() 
        {
            var osm = File.ReadAllText("/Users/luke/code/street-runner/src/StreetRunner/small-east-london.osm");

            var timer = Stopwatch.StartNew();
            var svg = Map.FromOsd(osm).ToSvgPath(500, 500);
            timer.Stop();
            
            Assert.InRange(timer.ElapsedMilliseconds, 0, 5000);
        }
    }
}
