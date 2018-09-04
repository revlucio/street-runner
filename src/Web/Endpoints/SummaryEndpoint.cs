using System;
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
            var totalLengthInMetres = _map.Streets.Sum(street => street.Length);
            var totalLengthCoveredInMetres = _map.Streets.Where(street => street.Covered).Sum(street => street.Length);
            var distanceCoveredPercentage = Math.Round(totalLengthCoveredInMetres / totalLengthInMetres * 100);

            var json = new
            {
                totalNumberOfStreets,
                numberOfCoveredStreets,
                coveredPercentage,
                totalLengthInMetres,
                totalLengthCoveredInMetres,
                distanceCoveredPercentage,
            };
            
            return JObject.FromObject(json).ToString();
        }
    }
}