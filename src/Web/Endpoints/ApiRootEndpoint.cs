using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json.Linq;

namespace StreetRunner.Web.Endpoints
{
    public class ApiRootEndpoint
    {
        public JObject Get(HttpRequest request, HttpResponse response, RouteData routeData)
        {
            return JObject.FromObject(new
            {
                urls = new[]
                {
                    $"{request.GetBaseUrl()}/api/map",
                    $"{request.GetBaseUrl()}/api/stats",
                },
                directory = Directory.GetCurrentDirectory(),
            });
        }
    }
}