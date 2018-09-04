using System.Collections.Generic;
using System.Linq;
using StreetRunner.Core.Mapping;

namespace StreetRunner.UnitTests.Web
{
    public class StubMap : IMap
    {
        private readonly List<Street> _streets = new List<Street>();
        private readonly List<IRun> _runs = new List<IRun>();

        public IEnumerable<Street> Streets => _streets;

        public IEnumerable<IRun> Runs => _runs;

        public void AddStreet(string street = "street")
        {
            _streets.Add(new Street(street, Enumerable.Empty<Point>()));
        }

        public void AddCoveredStreet()
        {
            var street = new Street("street", Enumerable.Empty<Point>());
            street.Covered = true;
            _streets.Add(street);
        }

        public void AddRunInLondon()
        {
            var points = new List<Point> { new Point(1, 1) };
            _runs.Add(new Run(points, "run"));
        }
        
        public void AddRunOutOfLondon()
        {
            var points = new List<Point> { new Point(10, 10) };
            _runs.Add(new Run(points, "run"));
        }
    }
}