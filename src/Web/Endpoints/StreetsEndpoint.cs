using System.Linq;
using Newtonsoft.Json.Linq;
using StreetRunner.Core.Mapping;

namespace StreetRunner.Web.Endpoints
{
    public class StreetsEndpoint
    {
        private readonly IMap _map;

        public StreetsEndpoint(IMap map)
        {
            _map = map;
        }

        public string Get()
        {
            var json = new
            {
                streets = _map.Streets.Select(street => new
                {
                    name = street.Name,
                    covered = street.Covered,
                    length = street.Length
                }),
            };
            
            return JObject.FromObject(json).ToString();
        }
    }
}