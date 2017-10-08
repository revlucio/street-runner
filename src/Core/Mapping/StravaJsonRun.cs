using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace StreetRunner.Core.Mapping
{
    public class StravaJsonRun : IRun
    {
        public StravaJsonRun(string json)
        {
            Points = JArray
                .Parse(json)
                .First
                .Value<JArray>("data")
                .Cast<JArray>()
                .Select(point => new Point((decimal)point[0], (decimal)point[1]));
        }

        public string Name => "Strava JSON Run";
        
        public IEnumerable<Point> Points { get; }
    }
}