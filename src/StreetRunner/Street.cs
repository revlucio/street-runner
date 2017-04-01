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

        public string ToSvgPath(int scaleLatTo, int scaleLonTo)
        {
            var offsetLatBy = Points.Select(p => p.Lat).Min();
            var offsetLonBy = Points.Select(p => p.Lon).Min();
            Console.WriteLine(offsetLatBy + " - " + offsetLonBy);

            var scaledPoints = Points
                .Select(p => new Point(p.Lat - offsetLatBy, p.Lon - offsetLonBy));

            var scaleLatBy = scaleLatTo / scaledPoints.Select(p => p.Lat).Max();
            var scaleLonBy = scaleLonTo / scaledPoints.Select(p => p.Lon).Max();

            var first = scaledPoints.First();

            var start = $"M {(int)(first.Lat*scaleLatBy)} {(int)(first.Lon*scaleLonBy)} ";
            var path = start + string.Join("", scaledPoints.Skip(1).Select(p => $"L {(int)(p.Lat*scaleLatBy)} {(int)(p.Lon*scaleLonBy)} "));
            return $"<path d=\"{path}\" stroke=\"black\" fill=\"transparent\"/>";
        }

        public string ToSvgPath(decimal offsetLatBy, decimal offsetLonBy, decimal scaleLatBy, decimal scaleLonBy)
        {
            var scaledPoints = Points
                .Select(p => new Point(p.Lat - offsetLatBy, p.Lon - offsetLonBy));

            var first = scaledPoints.First();

            var start = $"M {(int)(first.Lat*scaleLatBy)} {(int)(first.Lon*scaleLonBy)} ";
            var path = start + string.Join("", scaledPoints.Skip(1).Select(p => $"L {(int)(p.Lat*scaleLatBy)} {(int)(p.Lon*scaleLonBy)} "));
            return $"<path d=\"{path}\" stroke=\"black\" fill=\"transparent\"/>";
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