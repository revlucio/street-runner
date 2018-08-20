using System.Linq;
using Newtonsoft.Json.Linq;
using StreetRunner.Core.Mapping;

namespace StreetRunner.Web.Endpoints
{
    public class SummaryEndpoint
    {
        private readonly IMap _map;

        public SummaryEndpoint(IMap map)
        {
            _map = map;
        }

        public string Get()
        {
            var totalNumberOfStreets = _map.Streets.Count();
            var numberOfCoveredStreets = _map.Streets.Count(street => street.Covered);
            var coveredPercentage = numberOfCoveredStreets * 100m / totalNumberOfStreets;

            var json = new
            {
                totalNumberOfStreets,
                numberOfCoveredStreets,
                coveredPercentage,
            };
            
            return JObject.FromObject(json).ToString();
        }
    }
}