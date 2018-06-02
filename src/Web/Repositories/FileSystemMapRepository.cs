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
        private readonly ICoveredStreetCalculator _coveredStreetCalculator;

        public FileSystemMapRepository(
            IMapFinder mapFinder, 
            IRunRepository runRepository, 
            ICoveredStreetCalculator coveredStreetCalculator)
        {
            _mapFinder = mapFinder;
            _runRepository = runRepository;
            _coveredStreetCalculator = coveredStreetCalculator;
        }

        public Map Get(string mapName)
        {
            var osm = _mapFinder.FindMap(mapName);

            var map = MapFactory.FromOsm(osm, _runRepository.GetAll(), _coveredStreetCalculator);
            
            if (Directory.Exists(MapDirectory) == false)
            {
                Directory.CreateDirectory(MapDirectory);
            }

            return map;
        }
    }
}