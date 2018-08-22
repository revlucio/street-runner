using System.Collections.Generic;

namespace StreetRunner.Core.Mapping
{
    public interface IMap
    {
        IEnumerable<Street> Streets { get; }
        IEnumerable<IRun> Runs { get; }
    }
}