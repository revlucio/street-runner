using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace StreetRunner.Web
{
    public class FileSystemMapFinder : IMapFinder
    {
        public string FindMap(string filename)
        {
            return File.ReadAllText($"{Settings.MapFilesDirectory()}/{filename}.osm");
        }
        
        public string FindRun(string filename)
        {
            return File.ReadAllText($"{Settings.MapFilesDirectory()}/{filename}.gpx");
        }
        
        public IEnumerable<string> FindMapFiles()
        {
            return Directory
                .EnumerateFiles(Settings.MapFilesDirectory(), "*.osm")
                .Select(File.ReadAllText);
        }
        
        public IEnumerable<string> FindRuns()
        {
            return Directory
                .EnumerateFiles(Settings.MapFilesDirectory(), "*.gpx")
                .Select(File.ReadAllText);
        }
        
        public IEnumerable<string> FindMapFilenames()
        {
            return Directory
                .EnumerateFiles(Settings.MapFilesDirectory(), "*.osm")
                .Select(Path.GetFileNameWithoutExtension);
        }
    }
}