using System;
using StreetRunner.Core.Mapping;

namespace StreetRunner.Web.Endpoints
{
    public class StravaEndpoint
    {
        private readonly IHttpClient _httpClient;
        private readonly string _osm;

        public StravaEndpoint(IHttpClient httpClient, string osm)
        {
            _httpClient = httpClient;
            _osm = osm;
        }

        public string Get()
        {
            var url = "https://www.strava.com/api/v3/activities/1144313347/streams/latlng";
            var run = new StravaJsonRun(_httpClient.Get(url));

            var map = MapFactory.FromOsm(_osm, new []{run});
  
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