using System;
using System.Linq;
using StreetRunner.Core.Mapping;

namespace StreetRunner.Web.Repositories
{
    public static class RunExtensions
    {
        public static bool IsInLondon(this IRun run)
        {
            return run.Points.ToList().Any(point => Math.Abs(point.Lon) < 5);
        }
    }
}