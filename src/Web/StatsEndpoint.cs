using System;
using System.Linq;
using StreetRunner;

namespace Web
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
            var map = Map.FromOsd(this.osm);
            
            return map.Streets
                .GroupBy(street => street.Type)
                .OrderBy(type => type.Count())
                .Aggregate(string.Empty, (result, type) => $"{type.Count().ToString("0000")} - {type.Key}\n{result}");
        }
    }
}