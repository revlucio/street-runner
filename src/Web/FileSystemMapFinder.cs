using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace StreetRunner.Web
{
    public class FileSystemMapFinder : IMapFinder
    {
        private readonly string _mapFilesDirectory = Path.Combine(AppContext.BaseDirectory, "map-files");

        public string FindMap(string filename)
        {
            return File.ReadAllText($"{_mapFilesDirectory}/{filename}.osm");
        }
        
        public string FindRun(string filename)
        {
            return File.ReadAllText($"{_mapFilesDirectory}/{filename}.gpx");
        }
        
        public IEnumerable<string> FindMapFiles()
        {
            return Directory
                .EnumerateFiles(_mapFilesDirectory, "*.osm")
                .Select(File.ReadAllText);
        }
        
        public IEnumerable<string> FindRuns()
        {
            return Directory
                .EnumerateFiles(_mapFilesDirectory, "*.gpx")
                .Select(File.ReadAllText);
        }
        
        public IEnumerable<string> FindMapFilenames()
        {
            return Directory
                .EnumerateFiles(_mapFilesDirectory, "*.osm")
                .Select(Path.GetFileNameWithoutExtension);
        }
    }
}