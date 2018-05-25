using System.Collections.Generic;
using System.Linq;

namespace StreetRunner.Core.Mapping
{
    public class CoveredStreetCalculator : ICoveredStreetCalculator
    {
        public List<string> GetCoveredStreetsIds(IRun run, IEnumerable<Street> streets)
        {
            return streets
                .Where(street => street.IsCoveredBy(run))
                .Select(street => street.Name)
                .ToList();
        }
    }
}