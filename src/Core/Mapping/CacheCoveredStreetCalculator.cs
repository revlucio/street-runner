using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace StreetRunner.Core.Mapping
{
    public class CacheCoveredStreetCalculator : ICoveredStreetCalculator
    {
        private const string CacheDirectory = "/data/street-runner/runs";
        
        private readonly ICoveredStreetCalculator _innerCalculator;

        public CacheCoveredStreetCalculator(ICoveredStreetCalculator innerCalculator)
        {
            _innerCalculator = innerCalculator;
        }
        
        public List<string> GetCoveredStreetsIds(IRun run, IEnumerable<Street> streets)
        {
            var cacheFilename = Path.Combine(CacheDirectory, run.Id + ".json");

            if (File.Exists(cacheFilename))
            {
                var stuff = File.ReadAllText(cacheFilename);
                return JObject.Parse(stuff).Value<JArray>("coveredStreetsIds").Select(x => x.ToString()).ToList();
            }

            if (Directory.Exists(CacheDirectory) == false)
            {
                Directory.CreateDirectory(CacheDirectory);
            }
            
            var coveredStreetsIds = _innerCalculator.GetCoveredStreetsIds(run, streets);
            var json = JObject.FromObject(new
            {
                coveredStreetsIds,
            });
            File.WriteAllText(cacheFilename, json.ToString());
            
            return coveredStreetsIds;
        }
    }
}