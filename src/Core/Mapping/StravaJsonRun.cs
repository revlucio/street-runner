using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace StreetRunner.Core.Mapping
{
    public class StravaJsonRun : IRun
    {
        internal static StravaJsonRun Parse(string json, string id)
        {
            return new StravaJsonRun(json, id);
        }

        private StravaJsonRun(string json, string id)
        {
            Points = JArray
                .Parse(json)
                .First
                .Value<JArray>("data")
                .Cast<JArray>()
                .Select(point => new Point((decimal)point[0], (decimal)point[1]))
                .ToList();

            Id = id;
        }

        public IEnumerable<Point> Points { get; }
        public string Id { get; }
    }
}