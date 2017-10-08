using System;
using System.Collections.Generic;
using System.Linq;

namespace StreetRunner.Core.Mapping
{
    public static class MapExtensions
    {
        public static string ToSvg(this Map map, int scaleLatTo, int scaleLonTo)
        {
            var rect = GetBoundingRect(map);

            var offsetLatBy = rect.MinLat;
            var offsetLonBy = rect.MinLon;
            var scaleLatBy = scaleLatTo / (rect.MaxLat - offsetLatBy);
            var scaleLonBy = scaleLonTo / (rect.MaxLon - offsetLonBy);

            var streetPaths = map.Streets
                .Select(street =>
                {
                    var colour = street.Covered ? "yellow" : "black";
                    return ToSvgPath(scaleLatTo, offsetLatBy, offsetLonBy, scaleLatBy, scaleLonBy, street.Points, colour);
                });

            var runPaths = map.Runs
                .Select(run =>
                {
                    return ToSvgPath(scaleLatTo, offsetLatBy, offsetLonBy, scaleLatBy, scaleLonBy, run.Points, "red");
                });

            var paths = streetPaths.Concat(runPaths);
            var result = string.Join(Environment.NewLine, paths);
            return result;
        }
        
        private static Rect GetBoundingRect(Map map) 
        {
            var streetPoints = map.Streets.SelectMany(s => s.Points).ToList();
            var runPoints = map.Runs.SelectMany(r => r.Points).ToList();
            var allPoints = streetPoints.Concat(runPoints).ToList();
            if (allPoints.Any() == false) 
            {
                return null;
            }

            return new Rect
            {
                MinLat = allPoints.Select(p => p.Lat).Min(),
                MinLon = allPoints.Select(p => p.Lon).Min(),
                MaxLat = allPoints.Select(p => p.Lat).Max(),
                MaxLon = allPoints.Select(p => p.Lon).Max(),
            };
        }

        private static string ToSvgPath(int scaleLatTo, decimal offsetLatBy, decimal offsetLonBy, decimal scaleLatBy, decimal scaleLonBy, IEnumerable<Point> points, string colour)
        {
            var scaledPoints = points
                .Select(p => new Point(p.Lat - offsetLatBy, p.Lon - offsetLonBy))
                .Select(p => new Point(Math.Abs((p.Lat * scaleLatBy) - scaleLatTo), (int)(p.Lon * scaleLonBy)));

            var path = string.Join("", scaledPoints
                        .Select(p => $"L {p.Lon} {p.Lat} ")
                        )
                .Substring(1);

            return $"<path d=\"M{path}\" stroke=\"{colour}\" fill=\"transparent\"/>";
        }
    }
}