using System.Collections.Generic;
using System.Linq;
using StreetRunner.Core.Mapping;
using StreetRunner.Web.Endpoints;

namespace StreetRunner.Web.Repositories
{
    public class FileSystemRunRepository : IRunRepository
    {
        private readonly IMapFinder _mapFinder;

        public FileSystemRunRepository(IMapFinder mapFinder)
        {
            _mapFinder = mapFinder;
        }

        public IEnumerable<Run> GetAll()
        {
            return _mapFinder.FindRuns()
                .Select(Run.FromGpx);
        }
    }
}