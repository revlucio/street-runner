using System;
using System.Linq;
using StreetRunner.Core.Mapping;

namespace StreetRunner.Web.Endpoints
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
            var map = MapFactory.FromOsd(this.osm);
            if (this.gpx != null) {
                map.AddRun(Run.FromGpx(this.gpx));
            }
            
            return map.Streets
                .Aggregate(string.Empty, (result, street) => $"{result}{FormatStreetStatus(street)}{Environment.NewLine}");
        }

        private string FormatStreetStatus(Street street) 
        {
            var mark = street.Covered ? "X" : " ";
            return $"{mark} {street.Name}";
        }
    }
}