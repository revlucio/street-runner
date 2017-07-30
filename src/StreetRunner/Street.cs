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
        }

        public string Name { get; }
        public IEnumerable<Point> Points { get; }

        internal string ToSvgPath(int scaleLatTo, int scaleLonTo)
        {
            var offsetLatBy = Points.Select(p => p.Lat).Min();
            var offsetLonBy = Points.Select(p => p.Lon).Min();
            Console.WriteLine(offsetLatBy + " - " + offsetLonBy);

            var offsetPoints = Points
                .Select(p => new Point(p.Lat - offsetLatBy, p.Lon - offsetLonBy));

            var scaleLatBy = scaleLatTo / offsetPoints.Select(p => p.Lat).Max();
            var scaleLonBy = scaleLonTo / offsetPoints.Select(p => p.Lon).Max();

            var scaledPoints = offsetPoints
                .Select(p => new Point(p.Lat * scaleLatBy, Math.Abs((int)(p.Lon*scaleLonBy-scaleLonTo))));

            var first = scaledPoints.First();

            var start = $"M {first.Lat} {first.Lon} ";
            var path = string.Join("", scaledPoints
                .Select(p => $"L {p.Lat} {p.Lon} ")
                ).Substring(1);
            
            return $"<path d=\"M{path}\" stroke=\"black\" fill=\"transparent\"/>";
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