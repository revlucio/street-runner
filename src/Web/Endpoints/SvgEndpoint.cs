using System.Collections.Generic;
using System.Linq;
using StreetRunner.Core.Mapping;

namespace StreetRunner.Web.Endpoints
{
    public class SvgEndpoint
    {
        private readonly string _osm;
        private readonly IEnumerable<string> _gpxList;

        public SvgEndpoint(string osm, params string[] gpxList) : this(osm, gpxList.ToList())
        {
        }
        
        public SvgEndpoint(string osm, IEnumerable<string> gpxList)
        {
            _osm = osm;
            _gpxList = gpxList;
        }

        public string Get()
        {
            var map = MapFactory.FromOsm(_osm);
            _gpxList.ToList().ForEach(gpx => map.AddRun(Run.FromGpx(gpx)));
            
            var path = map.ToSvg(500, 500);

            return 
$@"<html>
<body>
<svg width=""500"" height=""500"">
{path}
</svg>
</body>
</html>
";
        }
    }
}