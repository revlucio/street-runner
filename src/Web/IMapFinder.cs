using System.Collections.Generic;

namespace StreetRunner.Web
{
    public interface IMapFinder
    {
        IEnumerable<string> FindMapFiles();
    }
}