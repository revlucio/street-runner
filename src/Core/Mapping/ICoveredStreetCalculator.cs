using System.Collections.Generic;

namespace StreetRunner.Core.Mapping
{
    public interface ICoveredStreetCalculator
    {
        List<string> GetCoveredStreetsIds(IRun run, IEnumerable<Street> streets);
    }
}