using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace StreetRunner
{
    public class Map
    {
        private IEnumerable<Street> _streets;
        private List<Run> _runs = new List<Run>();

        public IEnumerable<Street> Streets => _streets;

        public IEnumerable<Run> Runs => _runs;

        public void AddRun(string gpx)
        {
            var run = Run.FromGpx(gpx);
            AddRun(run);
        }

        public void AddRun(Run run) {
            _runs.Add(run);
            _streets = _streets.Select(street => {
                street.CheckIfCovered(run);
                return street;
            });
        }

        public static Map FromOsd(string osd)
        {
            var osdXml = XElement.Parse(osd);

            var nodes = osdXml.Elements("node");

            var streets = osdXml.Elements("way")
                .Where(way => way.Elements("tag").Any(tag => tag.Attribute("k").Value == "highway"))
                .Select(way => 
                {
                    var name = string.Empty;
                    if (way.Elements("tag").Any(tag => tag.Attribute("k").Value == "name")) {
                        name = way
                            .Elements("tag")
                            .Single(tag => tag.Attribute("k").Value == "name")
                            .Attribute("v").Value;
                    }

                    var highwayTag = way.Elements("tag").Single(tag => tag.Attribute("k").Value == "highway");
                    var type = highwayTag.Attribute("v").Value;

                    var points = way.Elements("nd")
                        .Select(node => 
                        {
                            var id = node.Attribute("ref").Value;
                            var match = nodes.Single(n => n.Attribute("id").Value == id);
                            var lat = decimal.Parse(match.Attribute("lat").Value);
                            var lon = decimal.Parse(match.Attribute("lon").Value);
                            return new Point(lat, lon);
                        });

                    return new Street(name, points, type);
                });

            return new Map(streets);
        }

        public class Rect 
        {
            public decimal MaxLat { get; set; }
            public decimal MaxLon { get; set; }
            public decimal MinLat { get; set; }
            public decimal MinLon { get; set; }
        }

        private Rect GetBoundingRect() 
        {
            var streetPoints = Streets.SelectMany(s => s.Points).ToList();
            var runPoints = Runs.SelectMany(r => r.Points).ToList();
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

        private Rect GetEastLondonBoundingRect() 
        {
            return new Rect 
            {
                MaxLat = 51.5156111m,
                MinLat = 51.5007243m,
                MaxLon = -0.0418513m,
                MinLon = -0.0851122m,
            };
        }

        public string ToSvgPath(int scaleLatTo, int scaleLonTo)
        {
            var rect = GetBoundingRect();

            var offsetLatBy = rect.MinLat;
            var offsetLonBy = rect.MinLon;
            var scaleLatBy = scaleLatTo / (rect.MaxLat - offsetLatBy);
            var scaleLonBy = scaleLonTo / (rect.MaxLon - offsetLonBy);

            var streetPaths = Streets.Select(street => {
                var colour = street.Covered ? "yellow" : "black";
                return ToSvgPath(scaleLatTo, offsetLatBy, offsetLonBy, scaleLatBy, scaleLonBy, street.Points, colour);
            });
            var runPaths = Runs.Select(run => {
                return ToSvgPath(scaleLatTo, offsetLatBy, offsetLonBy, scaleLatBy, scaleLonBy, run.Points, "red");
            });
            var paths = streetPaths.Concat(runPaths);
            
            return String.Join(Environment.NewLine, paths);
        }

        private static string ToSvgPath(int scaleLatTo, decimal offsetLatBy, decimal offsetLonBy, decimal scaleLatBy, decimal scaleLonBy, IEnumerable<Point> points, string colour)
        {
            var scaledPoints = points
                            .Select(p => new Point(p.Lat - offsetLatBy, p.Lon - offsetLonBy))
                            .Select(p => new Point(Math.Abs((p.Lat * scaleLatBy) - scaleLatTo), (int)(p.Lon * scaleLonBy)));

            var first = scaledPoints.First();

            var path = string.Join("", scaledPoints
                .Select(p => $"L {p.Lon} {p.Lat} ")
                ).Substring(1);

            return $"<path d=\"M{path}\" stroke=\"{colour}\" fill=\"transparent\"/>";
        }

        public Map(IEnumerable<Street> streets)
        {
            _streets = streets;
        }
    }
}