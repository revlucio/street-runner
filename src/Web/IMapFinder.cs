using System.Collections.Generic;

namespace StreetRunner.Web
{
    public interface IMapFinder
    {
        IEnumerable<string> FindMapFiles();
        IEnumerable<string> FindMapFilenames();
        string FindMap(string filename);
        string FindRun(string filename);
        IEnumerable<string> FindRuns();
    }
}