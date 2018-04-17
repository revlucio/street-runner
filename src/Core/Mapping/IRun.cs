using System.Collections.Generic;

namespace StreetRunner.Core.Mapping
{
    public interface IRun
    {
        string Name { get; }
        IEnumerable<Point> Points { get; }
        string Time { get; set; }
    }
}