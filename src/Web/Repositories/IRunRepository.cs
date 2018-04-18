using System.Collections.Generic;
using StreetRunner.Core.Mapping;

namespace StreetRunner.Web.Repositories
{
    public interface IRunRepository
    {
        IEnumerable<IRun> GetAll();
    }
}