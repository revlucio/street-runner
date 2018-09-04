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

        public void AddStreet(string name = "street", decimal length = 10m, bool covered = false)
        {
            var points = new List<Point>
            {
                new Point(0, 0),
                new Point(0.000009m * length, 0),
            };
            var street = new Street(name, points);
            street.Covered = covered;
            _streets.Add(street);
        }

        public void AddCoveredStreet()
        {
            AddStreet(covered: true);
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