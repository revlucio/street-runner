using System.Linq;
using StreetRunner.Core.Mapping;
using StreetRunner.Web.Endpoints;

namespace StreetRunner.Web.Repositories
{
    public class FileSystemMapRepository : IMapRepository
    {
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
            var map = MapFactory.FromOsm(osm, _runRepository.GetAll());

            return map;
        }
    }
}