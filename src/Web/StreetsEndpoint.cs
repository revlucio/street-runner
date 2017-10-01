using System;
using System.Linq;
using StreetRunner;

namespace Web
{
    public class StreetsEndpoint
    {
        private string osm;
        private string gpx;

        public StreetsEndpoint(string osm)
        {
            this.osm = osm;
        }

        public StreetsEndpoint(string osm, string gpx)
        {
            this.osm = osm;
            this.gpx = gpx;
        }

        public string Get()
        {
            var map = Map.FromOsd(this.osm);
            if (this.gpx != null) {
                map.AddRun(Run.FromGpx(this.gpx));
            }
            
            return map.Streets
                .Aggregate(string.Empty, (result, street) => result + street.Name + Environment.NewLine);
        }
    }
}