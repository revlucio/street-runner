using System.Linq;
using StreetRunner.Core.Mapping;

namespace StreetRunner.Web.Endpoints
{
    public class StatsEndpoint
    {
        private string osm;

        public StatsEndpoint(string osm)
        {
            this.osm = osm;
        }

        public string Get()
        {
            var map = MapFactory.FromOsm(this.osm);
            
            return map.Streets
                .GroupBy(street => street.Type)
                .OrderBy(type => type.Count())
                .Aggregate(string.Empty, (result, type) => $"{type.Count().ToString("0000")} - {type.Key}\n{result}");
        }
    }
}