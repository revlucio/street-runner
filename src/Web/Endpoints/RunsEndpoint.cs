using System.Linq;
using Newtonsoft.Json.Linq;
using StreetRunner.Core.Mapping;
using StreetRunner.Web.Repositories;

namespace StreetRunner.Web.Endpoints
{
    public class RunsEndpoint
    {
        private readonly IMap _map;

        public RunsEndpoint(IMap map)
        {
            _map = map;
        }

        public string Get()
        {
            var json = new
            {
                runs = _map.Runs.Select(run => new
                {
                    isInLondon = run.IsInLondon(),
                })
            };
            
            return JObject.FromObject(json).ToString();
        }
    }
}