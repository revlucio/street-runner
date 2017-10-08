using StreetRunner.Core.Mapping;

namespace Web.Endpoints
{
    public class SvgEndpoint
    {
        private string osm;
        private string gpx;

        public SvgEndpoint(string osm)
        {
            this.osm = osm;
        }

        public SvgEndpoint(string osm, string gpx)
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