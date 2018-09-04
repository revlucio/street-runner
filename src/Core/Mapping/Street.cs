using System.Collections.Generic;
using System.Linq;

namespace StreetRunner.Core.Mapping
{
    public class Street
    {
        private readonly string _name;

        public Street(string name, IEnumerable<Point> points, string type = "?")
        {
            _name = name;
            Points = points;
            Covered = false;
            Type = type;
        }

        public string Name => string.IsNullOrWhiteSpace(_name)
            ? Points.Aggregate(string.Empty, (result, point) => $"{result}{point.Lat}{point.Lon}")
            : _name;

        public string Type { get; }
        public IEnumerable<Point> Points { get; }
        public bool Covered { get; set; }

        public decimal Length => Points.Count() < 2 
            ? 0 
            : Points.First().CalculateDistanceInMetres(Points.Last());

        internal bool IsCoveredBy(IRun run)
        {
            if (Covered)
            {
                return true;
            }
            
            if (Points.Any(streetPoint 
                => run.Points.Any(runPoint 
                    => streetPoint.CalculateDistanceInMetres(runPoint) < 3)))
            {
                return true;
            }

            return false;
        }
    }
}