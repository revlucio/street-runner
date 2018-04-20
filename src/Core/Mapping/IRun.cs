using System.Collections.Generic;

namespace StreetRunner.Core.Mapping
{
    public interface IRun
    {
        IEnumerable<Point> Points { get; }
        string Id { get; }
    }
}