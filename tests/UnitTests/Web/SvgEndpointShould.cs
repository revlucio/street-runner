using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using StreetRunner.Web;
using StreetRunner.Web.Endpoints;
using StreetRunner.Web.Repositories;
using Xunit;

namespace StreetRunner.UnitTests.Web
{
    public class SvgEndpointShould
    {
        [Fact]
        public void OutputSvg()
        {
            var expected = 
@"<html>
<body>
<svg width=""500"" height=""500"">
<path d=""M 0 500 L 500 0 "" stroke=""yellow"" fill=""transparent""/>
<path d=""M 0 500 L 500 0 "" stroke=""red"" fill=""transparent""/>
</svg>
</body>
</html>
";

            var osm = @"
<osm>
 <node id=""1"" lat=""10"" lon=""10""/>
 <node id=""2"" lat=""20"" lon=""20""/>
 <way>
  <nd ref=""1""/>
  <nd ref=""2""/>
  <tag k=""highway"" v=""secondary""/>
 </way>
</osm>            
";

            var gpx = @"<gpx>
<metadata>
  <time>2018-04-16T15:05:30Z</time>
</metadata>
<trk>
    <name>Test Run</name>
    <trkseg>
      <trkpt lat=""10"" lon=""10""></trkpt>
      <trkpt lat=""20"" lon=""20""></trkpt>
    </trkseg>
</trk>
</gpx>";

            
            var stubFinder = new StubMapFinder(
                new Dictionary<string, string>{ { "mapName", osm }}, 
                new List<string> { gpx });
            var actual = new SvgEndpoint("mapName", new FileSystemMapRepository(stubFinder, new FileSystemRunRepository(stubFinder))).Get();

            Assert.Equal(expected, actual);
        }

        [Fact(Skip="not ready yet")]
        public async Task Request()
        {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer 93944545f252e152f5aeb0128fcca26760eadd01");

                    var response = await client.GetAsync("https://www.strava.com/api/v3/activities/1144313347/streams/latlng");
                    var content = await response.Content.ReadAsStringAsync();
                    
                    Assert.Equal(response.StatusCode, HttpStatusCode.OK);
                    Assert.True(content.Length > 0);
                    Console.WriteLine("output: " +content);   
                }

        }
    }
}
