using System;
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
        private readonly string _token;

        public StravaRunRepository(IHttpClient httpClient, IHttpClient cacheHttpClient, string token)
        {
            _httpClient = httpClient;
            _cacheHttpClient = cacheHttpClient;
            _token = token;
        }
        
        public IEnumerable<IRun> GetAll()
        {

            return JArray
                .Parse(_httpClient.Get($"https://www.strava.com/api/v3/athlete/activities?access_token={_token}"))
                .Select(activity => activity.Value<string>("id"))
                .Take(5)
                .Select(activityId => new
                {
                    id = activityId,
                    json = _cacheHttpClient.Get($"https://www.strava.com/api/v3/activities/{activityId}/streams/latlng?access_token={_token}")
                })
                .Select(activity => new StravaJsonRun(activity.json, activity.id))
                .Where(IsInLondon);
        }

        private bool IsInLondon(StravaJsonRun run)
        {
            return run.Points.ToList().Any(point => Math.Abs(point.Lon) < 5);
        }
    }
}