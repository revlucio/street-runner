using System.Collections.Generic;
using System.Linq;

namespace StreetRunner.Core.Mapping
{
    public class Street
    {
        public Street(string name, IEnumerable<Point> points, string type = "?")
        {
            Name = name;
            Points = points;
            Covered = false;
            Type = type;
        }

        public string Name { get; }
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
                    => streetPoint.CalculateDistanceInMetres(runPoint) < 110))) 
            {
                Covered = true;
            }
        }
    }
}