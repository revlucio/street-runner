using System;
using System.Collections.Generic;
using System.Linq;

namespace StreetRunner
{
    public class Street
    {
        public Street(string name, IEnumerable<Point> points)
        {
            Name = name;
            Points = points;
            Covered = false;
        }

        public string Name { get; }
        public IEnumerable<Point> Points { get; }
        public bool Covered { get; private set; }

        internal void CheckIfCovered(Run run)
        {
            if (this.Points.Select(p => p.Lat).All(point => run.Points.Select(p => p.Lat).Contains(point))) {
                this.Covered = true;
            }
        }
    }

    public class Point
    {
        public Point(decimal lat, decimal lon) 
        {
            Lat = lat;
            Lon = lon;
        }

        public decimal Lat { get; }
        public decimal Lon { get; }
    }
}