using StreetRunner.Core.Mapping;

namespace StreetRunner.Web.Repositories
{
    public class FileSystemMapRepository : IMapRepository
    {
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
            var runs = _runRepository.GetAll();
            return MapFactory.FromOsm(osm, runs, _coveredStreetCalculator);
        }
    }
}