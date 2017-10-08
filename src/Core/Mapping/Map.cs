using System.Collections.Generic;
using System.Linq;

namespace StreetRunner.Core.Mapping
{
    public class Map
    {
        private readonly List<IRun> _runs = new List<IRun>();

        public IEnumerable<Street> Streets { get; private set; }

        public IEnumerable<IRun> Runs => _runs;

        public Map(IEnumerable<Street> streets)
        {
            Streets = streets;
        }

        public void AddRun(string gpx)
        {
            var run = Run.FromGpx(gpx);
            AddRun(run);
        }

        public void AddRun(IRun run) {
            _runs.Add(run);
            Streets = Streets.Select(street => {
                street.CheckIfCovered(run);
                return street;
            })
            .ToList();
        }
    }
}