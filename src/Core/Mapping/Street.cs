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

        internal void CheckIfCovered(IRun run)
        {
            if (Covered)
            {
                return;
            }
            
            if (Points.Any(streetPoint 
                => run.Points.Any(runPoint 
                    => streetPoint.CalculateDistanceInMetres(runPoint) < 3))) 
            {
                Covered = true;
            }
        }
    }
}