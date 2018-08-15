using StreetRunner.Core.Mapping;
using StreetRunner.Web.Repositories;

namespace StreetRunner.Web.Endpoints
{
    public class SvgEndpoint
    {
        private readonly string _mapName;
        private readonly IMapRepository _mapRepository;

        public SvgEndpoint(string mapName, IMapRepository mapRepository)
        {
            _mapName = mapName;
            _mapRepository = mapRepository;
        }

        public string Get()
        {
            var path = _mapRepository.Get(_mapName).ToSvgMaintainAspectRatio(1000);

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