using StreetRunner.Core.Mapping;
using StreetRunner.Web.Repositories;

namespace StreetRunner.Web.Endpoints
{
    public class SvgEndpoint
    {
        private readonly IMapRepository _mapRepository;

        public SvgEndpoint(IMapRepository mapRepository)
        {
            _mapRepository = mapRepository;
        }

        public string Get(string mapName)
        {
            var path = _mapRepository.Get(mapName).ToSvgMaintainAspectRatio(1000);

            return 
$@"<html>
<body>
<svg width=""100%"" height=""100%"">
{path}
</svg>
</body>
</html>
";
        }
    }
}