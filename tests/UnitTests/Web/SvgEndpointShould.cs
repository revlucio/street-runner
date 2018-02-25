using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using StreetRunner.Web.Endpoints;
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
<path d=""M 0 500 L 500 0.00000000000000000000000001 "" stroke=""black"" fill=""transparent""/>
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

            var actual = new SvgEndpoint(osm).Get();

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
