using System;

namespace StreetRunner.Tests
{
    public class SvgEndpoint
    {
        private string osm;

        public SvgEndpoint(string osm)
        {
            this.osm = osm;
        }

        public string Get()
        {
            var path = Map.FromOsd(this.osm).ToSvgPath(500, 500);

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