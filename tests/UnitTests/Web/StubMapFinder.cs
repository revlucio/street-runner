using System.Collections.Generic;
using StreetRunner.Web;
using StreetRunner.Web.Endpoints;

namespace StreetRunner.UnitTests.Web
{
    public class StubMapFinder : IMapFinder
    {
        private readonly string _osm;

        public StubMapFinder(string osm)
        {
            _osm = osm;
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
            throw new System.NotImplementedException();
        }

        public string FindRun(string filename)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<string> FindRuns()
        {
            throw new System.NotImplementedException();
        }
    }
}