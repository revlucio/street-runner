using System.Collections.Generic;
using System.Linq;
using StreetRunner.Core.Mapping;

namespace StreetRunner.UnitTests.Web
{
    public class StubMap : IMap
    {
        private readonly List<Street> _streets = new List<Street>();

        public IEnumerable<Street> Streets => _streets;

        public void AddStreet()
        {
            _streets.Add(new Street("street", Enumerable.Empty<Point>()));
        }

        public void AddCoveredStreet()
        {
            var street = new Street("street", Enumerable.Empty<Point>());
            street.Covered = true;
            _streets.Add(street);
        }
    }
}