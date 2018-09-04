using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using StreetRunner.Core.Mapping;

namespace StreetRunner.Web.Repositories
{
    public class StravaRunRepository : IRunRepository
    {
        private readonly IHttpClient _httpClient;
        private readonly IHttpClient _cacheHttpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public StravaRunRepository(IHttpClient httpClient, IHttpClient cacheHttpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _cacheHttpClient = cacheHttpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        public IEnumerable<IRun> GetAll()
        {
            var token = _httpContextAccessor.HttpContext.Request.Cookies["strava-token"];
            
            return JArray
                .Parse(_httpClient.Get($"{Settings.StravaUrl}/api/v3/athlete/activities?access_token={token}"))
                .Select(activity => activity.Value<string>("id"))
                .Take(1)
                .Select(activityId => new
                {
                    id = activityId,
                    json = _cacheHttpClient.Get($"{Settings.StravaUrl}/api/v3/activities/{activityId}/streams/latlng?access_token={token}")
                })
                .Select(activity => new StravaRunParser(activity.json, activity.id))
                .Where(run => run.IsValid())
                .Select(run => run.Value)
                .Where(run => run.IsInLondon());
        }
    }
}