using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks.Dataflow;
using Newtonsoft.Json.Linq;

namespace StreetRunner.Core.Mapping
{
    public class Map : IMap
    {
        private readonly List<IRun> _runs;

        public IEnumerable<Street> Streets { get; }

        public IEnumerable<IRun> Runs => _runs;

        public Map(IEnumerable<Street> streets) : this(streets, Enumerable.Empty<IRun>())
        {
        }

        public Map(
            IEnumerable<Street> streets, 
            IEnumerable<IRun> runs, 
            ICoveredStreetCalculator coveredStreetCalculator = null)
        {
            _runs = runs.ToList();
            Streets = streets
                //.Where(street => street.Length > 50m) // exclude smaller streets
                .ToList();
            
            coveredStreetCalculator = coveredStreetCalculator ?? new CoveredStreetCalculator();
            
            _runs
                .ForEach(run =>
                {
                    var coveredStreets = coveredStreetCalculator.GetCoveredStreetsIds(run, Streets);
                    Streets.ForEach(street => street.Covered = coveredStreets.Contains(street.Name));
                });
        }

        public JObject ToJson()
        {
            return JObject.FromObject(new
            {
                runIds = _runs.Select(run => run.Id),
                coveredStreets = Streets
                    .Where(street => street.Covered)
                    .Select(street => street.Name)
            });
        }
    }
}