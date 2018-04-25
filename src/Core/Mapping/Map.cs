using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace StreetRunner.Core.Mapping
{
    public class Map
    {
        private readonly List<IRun> _runs = new List<IRun>();
        private readonly List<string> _cachedRuns = new List<string>();

        public IEnumerable<Street> Streets { get; private set; }

        public IEnumerable<IRun> Runs => _runs;

        public Map(IEnumerable<Street> streets) : this(streets, Enumerable.Empty<IRun>())
        {
        }
        
        public Map(IEnumerable<Street> streets, IEnumerable<IRun> runs)
        {
            Streets = streets;
            runs.ForEach(AddRun);
        }

        public Map(IEnumerable<Street> streets, IEnumerable<IRun> runs, JObject mapJson)
        {
            var coveredStreets = GetListOrEmpty(mapJson, "coveredStreets");
            _cachedRuns = GetListOrEmpty(mapJson, "runIds");
            
            Streets = streets
                .Select(street =>
                {
                    if (coveredStreets.Contains(street.Name))
                    {
                        street.Covered = true;
                    }
                    return street;
                });
            
            runs.ForEach(AddRun);
        }

        private static List<string> GetListOrEmpty(JObject mapJson, string propertyName)
        {
            if (mapJson.Properties().Any(property => property.Name == propertyName))
            {
                return mapJson.Value<JArray>(propertyName).Values<string>().ToList();    
            }

            return new List<string>();
        }

        private void AddRun(IRun run) {
            _runs.Add(run);
            Streets = Streets
                .Select(street => {
                    if (_cachedRuns.Contains(run.Id) == false)
                    {
                        street.CheckIfCovered(run);    
                    }
                    
                    return street;
                })
                .ToList();
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