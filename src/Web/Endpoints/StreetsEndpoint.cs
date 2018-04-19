using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using StreetRunner.Core.Mapping;

namespace StreetRunner.Web.Endpoints
{
    public class StreetsEndpoint
    {
        private readonly string _osm;
        private readonly IEnumerable<string> _gpxList;

        public StreetsEndpoint(string osm) : this(osm, Enumerable.Empty<string>())
        {
        }

        public StreetsEndpoint(string osm, string gpx) : this(osm, new []{gpx})
        {
        }

        private StreetsEndpoint(string osm, IEnumerable<string> gpxList)
        {
            _osm = osm;
            _gpxList = gpxList;
        }

        public string Get()
        {
            var map = MapFactory.FromOsm(_osm, _gpxList.Select(Run.FromGpx));
            
            return map.Streets.Aggregate(
                    string.Empty, 
                    (result, street) => $"{result}{FormatStreetStatus(street)}{Environment.NewLine}");
        }

        private string FormatStreetStatus(Street street) 
        {
            var mark = street.Covered ? "X" : " ";
            return $"{mark} {street.Name}";
        }
    }
}