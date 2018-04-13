using System.Linq;
using StreetRunner.Core.Mapping;

namespace StreetRunner.Web.Endpoints
{
    public class StatsEndpoint
    {
        private readonly IMapFinder _mapFinder;
        
        public StatsEndpoint(IMapFinder mapFinder)
        {
            _mapFinder = mapFinder;
        }

        public string Get()
        {
            var map = MapFactory.FromOsm(_mapFinder.FindMapFiles().First());
            
            return map.Streets
                .GroupBy(street => street.Type)
                .OrderBy(type => type.Count())
                .Aggregate(string.Empty, (result, type) => $"{type.Count():0000} - {type.Key}\n{result}");
        }
    }
}