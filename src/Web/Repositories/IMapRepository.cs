using StreetRunner.Core.Mapping;

namespace StreetRunner.Web.Repositories
{
    public interface IMapRepository
    {
        Map Get(string mapName);
    }
}