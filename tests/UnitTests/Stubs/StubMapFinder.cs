using System.Collections.Generic;
using StreetRunner.Web;
using StreetRunner.Web.Endpoints;

namespace StreetRunner.UnitTests.Web
{
    public class StubMapFinder : IMapFinder
    {
        private readonly string _osm;
        private readonly Dictionary<string, string> _osmDict;
        private readonly IEnumerable<string> _gpxList;

        public StubMapFinder(string osm, IEnumerable<string> gpxList = null)
        {
            _osm = osm;
            _gpxList = gpxList;
        }
        
        public StubMapFinder(Dictionary<string, string> osmDict, IEnumerable<string> gpxList)
        {
            _osmDict = osmDict;
            _gpxList = gpxList;
        }

        public IEnumerable<string> FindMapFiles()
        {
            return new[] {_osm};
        }

        public IEnumerable<string> FindMapFilenames()
        {
            throw new System.NotImplementedException();
        }

        public string FindMap(string filename)
        {
            return _osmDict[filename];
        }

        public string FindRun(string filename)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<string> FindRuns()
        {
            return _gpxList;
        }
    }
}