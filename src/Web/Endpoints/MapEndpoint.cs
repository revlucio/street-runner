using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json.Linq;

namespace StreetRunner.Web.Endpoints
{
    public class MapEndpoint
    {
        private static IMapFinder _mapFinder;

        public MapEndpoint(IMapFinder mapFinder)
        {
            _mapFinder = mapFinder;
        }

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
            var mapFiles = _mapFinder.FindMapFilenames()
                .Select(mapFile => $"{Settings.UrlRoot}/api/map/{mapFile}");
            return mapFiles;
        }
    }
}