using System.IO;
using System.Linq;
using System.Net;
using Newtonsoft.Json.Linq;
using StreetRunner.Core.Mapping;

namespace StreetRunner.Web.Repositories
{
    public class FileSystemMapRepository : IMapRepository
    {
        private const string MapDirectory = "/data/street-runner/maps";
        
        private readonly IMapFinder _mapFinder;
        private readonly IRunRepository _runRepository;

        public FileSystemMapRepository(IMapFinder mapFinder, IRunRepository runRepository)
        {
            _mapFinder = mapFinder;
            _runRepository = runRepository;
        }

        public Map Get(string mapName)
        {
            var osm = _mapFinder.FindMap(mapName);
            
            var mapJsonPath = Path.Combine(MapDirectory, mapName + ".json");
            var mapJson = File.Exists(mapJsonPath) 
                ? JObject.Parse(File.ReadAllText(mapJsonPath)) 
                : new JObject();
            
            var map = MapFactory.FromOsm(osm, _runRepository.GetAll(), mapJson);
            
            if (Directory.Exists(MapDirectory) == false)
            {
                Directory.CreateDirectory(MapDirectory);
            }
            File.WriteAllText(mapJsonPath, map.ToJson().ToString());

            return map;
        }
    }
}