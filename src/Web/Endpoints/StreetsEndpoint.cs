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
                streets = _map.Streets.Select(street => street.Name),
            };
            
            return JObject.FromObject(json).ToString();
            
//            return map.Streets.Aggregate(
//                    string.Empty, 
//                    (result, street) => $"{result}{FormatStreetStatus(street)}{Environment.NewLine}");
        }

        private string FormatStreetStatus(Street street) 
        {
            var mark = street.Covered ? "X" : " ";
            return $"{mark} {street.Name}";
        }
    }
}