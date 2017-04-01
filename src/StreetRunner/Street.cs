using System.Collections.Generic;

namespace tests
{
    public class Street
    {
        public Street(string name, IEnumerable<Point> points)
        {
            Name = name;
            Points = points;
        }

        public string Name { get; }
        public IEnumerable<Point> Points { get; }
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