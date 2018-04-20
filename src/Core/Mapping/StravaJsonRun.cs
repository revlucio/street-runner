using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace StreetRunner.Core.Mapping
{
    public class StravaJsonRun : IRun
    {
        public StravaJsonRun(string json, string id="default")
        {
            Points = JArray
                .Parse(json)
                .First
                .Value<JArray>("data")
                .Cast<JArray>()
                .Select(point => new Point((decimal)point[0], (decimal)point[1]));

            Id = id;
        }

        public IEnumerable<Point> Points { get; }
        public string Id { get; }
    }
}