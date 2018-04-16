using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace StreetRunner.Web.Endpoints
{
    public class MapEndpoint
    {
        public string Get()
        {
            return string.Join(Environment.NewLine, MapFiles());
        }

        public JObject GetJson()
        {
            return JObject.FromObject(new
            {
                maps = MapFiles()
            });
        }

        private static IEnumerable<string> MapFiles()
        {
            var mapDirectory = Path.Combine(AppContext.BaseDirectory, "map-files");

            var mapFiles = Directory
                .EnumerateFiles(mapDirectory, "*.osm")
                .Select(mapFile => mapFile.Replace(mapDirectory, string.Empty))
                .Select(mapFile => mapFile.Replace(".osm", string.Empty))
                .Select(mapFile => mapFile.Replace("/", string.Empty))
                .Select(mapFile => $"{Settings.UrlRoot}/api/map/{mapFile}");
            return mapFiles;
        }
    }
}