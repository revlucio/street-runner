using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json.Linq;

namespace StreetRunner.Web.Endpoints
{
    public class MapEndpoint
    {
        private static IMapFinder _mapFinder;

        public MapEndpoint(IMapFinder mapFinder)
        {
            _mapFinder = mapFinder;
        }

        public JObject GetJson(HttpRequest request, HttpResponse response, RouteData routeData)
        {
            return JObject.FromObject(new
            {
                maps = _mapFinder.FindMapFilenames()
                    .Select(mapFile => $"{request.GetBaseUrl()}/api/map/{mapFile}")
            });
        }
    }
}