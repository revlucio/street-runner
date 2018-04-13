using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace StreetRunner.Web
{
    public class FileSystemMapFinder : IMapFinder
    {
        public IEnumerable<string> FindMapFiles()
        {
            return Directory
                .EnumerateFiles(Settings.MapFilesDirectory(), "*.osm")
                .Select(File.ReadAllText);
        }
    }
}