using System;
using System.IO;
using System.Linq;
using StreetRunner;

namespace Web
{
    public class MapEndpoint
    {
        public string Get()
        {
            var mapDirectory = Path.Combine(AppContext.BaseDirectory, "map-files");
            var mapFiles = Directory
                .EnumerateFiles(mapDirectory)
                .Select(mapFile => mapFile.Replace(mapDirectory, string.Empty))
                .Select(mapFile => mapFile.Replace(".osm", string.Empty))
                .Select(mapFile => mapFile.Replace("/", string.Empty))
                .Select(mapFile => $"http://localhost:5000/map/{mapFile}");

            return string.Join(Environment.NewLine, mapFiles);
        }
    }
}