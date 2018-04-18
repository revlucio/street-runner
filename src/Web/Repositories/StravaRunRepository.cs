using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using StreetRunner.Core.Mapping;

namespace StreetRunner.Web.Repositories
{
    public class StravaRunRepository : IRunRepository
    {
        public IEnumerable<IRun> GetAll()
        {
            using (var client = new HttpClient
            {
                BaseAddress = new Uri("https://www.strava.com")
            })
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("Authorization", "Bearer 93944545f252e152f5aeb0128fcca26760eadd01");

                var response = client.GetAsync("/api/v3/athlete/activities").Result;
                var content = response.Content.ReadAsStringAsync().Result;

                var activityIds = JArray.Parse(content)
                    .Select(activity => activity.Value<string>("id"))
                    .ToList();
                
                response = client.GetAsync($"/api/v3/activities/{activityIds.First()}/streams/latlng").Result;
                content = response.Content.ReadAsStringAsync().Result;

                var run = new StravaJsonRun(content);
                
                return new List<IRun> { run };
            }
        }
    }
}