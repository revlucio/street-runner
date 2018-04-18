using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using StreetRunner.Core.Mapping;

namespace StreetRunner.Web.Repositories
{
    public class StravaRunRepository : IRunRepository
    {
        private readonly IHttpClient _httpClient;
        private readonly IHttpClient _cacheHttpClient;

        public StravaRunRepository(IHttpClient httpClient, IHttpClient cacheHttpClient)
        {
            _httpClient = httpClient;
            _cacheHttpClient = cacheHttpClient;
        }
        
        public IEnumerable<IRun> GetAll()
        {
            var activityIds = JArray
                    .Parse(_httpClient.Get("/api/v3/athlete/activities"))
                    .Select(activity => activity.Value<string>("id"))
                    .ToList();

            var activityJson = _cacheHttpClient.Get($"/api/v3/activities/{activityIds.First()}/streams/latlng");
            
            var run = new StravaJsonRun(activityJson);
                
            return new List<IRun> { run };
        }
    }
}