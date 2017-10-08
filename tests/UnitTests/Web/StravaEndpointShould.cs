using System.Collections.Generic;
using StreetRunner.Web.Endpoints;
using Xunit;

namespace StreetRunner.UnitTests.Web
{
    public class StravaEndpointShould
    {
        [Fact]
        public void OutputSvg()
        {
            var expected = 
                @"<html>
<body>
<svg width=""500"" height=""500"">
<path d=""M 0 500 L 500 0.00000000000000000000000001 "" stroke=""yellow"" fill=""transparent""/>
<path d=""M 0 500 L 500 0.00000000000000000000000001 "" stroke=""red"" fill=""transparent""/>
</svg>
</body>
</html>
";

            var osm = @"
<osm>
 <node id=""111"" lat=""11.1"" lon=""22.2""/>
 <node id=""222"" lat=""33.3"" lon=""44.4""/>
 <way>
  <nd ref=""111""/>
  <nd ref=""222""/>
  <tag k=""highway"" v=""secondary""/>
 </way>
</osm>            
";
            var stravaActivityJson = @"
[
    {
        ""data"": [
            [ 11.1, 22.2 ],
            [ 33.3, 44.4 ]
        ]
    }
]";
            var stubHttpClient = new StubHttpClient();
            stubHttpClient.SetupGet("https://www.strava.com/api/v3/activities/1144313347/streams/latlng", stravaActivityJson);

            var actual = new StravaEndpoint(stubHttpClient, osm).Get();

            Assert.Equal(expected, actual);
        }
    }

    public class StubHttpClient : IHttpClient
    {
        private readonly Dictionary<string, string> _responses = new Dictionary<string, string>();
        
        public void SetupGet(string url, string response)
        {
            _responses.Add(url, response);
        }

        public string Get(string url)
        {
            return _responses[url];
        }
    }
}
