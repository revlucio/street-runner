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

        public IEnumerable<Street> Streets => _streets
            // .Where(s => s.Name == "Alie Street" || s.Name == "Prescot Street" || s.Name == "Leman Street")
            ;

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
                .Where(way => way.Elements("tag").Any(tag => tag.Attribute("k").Value == "name"))
                .Select(way => 
                {
                    var nameTag = way.Elements("tag").Single(tag => tag.Attribute("k").Value == "name");
                    var name = nameTag.Attribute("v").Value;

                    var points = way.Elements("nd")
                        .Select(node => 
                        {
                            var id = node.Attribute("ref").Value;
                            var match = nodes.Single(n => n.Attribute("id").Value == id);
                            var lat = decimal.Parse(match.Attribute("lat").Value);
                            var lon = decimal.Parse(match.Attribute("lon").Value);
                            return new Point(lat, lon);
                        });

                    return new Street(name, points);
                });

            return new Map(streets);
        }

        public string ToSvgPath(int scaleLatTo, int scaleLonTo)
        {
            var streetPoints = Streets.SelectMany(s => s.Points);
            var runPoints = Runs.SelectMany(r => r.Points);
            var allPoints = streetPoints.Concat(runPoints);
            if (allPoints.Any() == false) 
            {
                return "";
            }

            var offsetLatBy = allPoints.Select(p => p.Lat).Min();
            var offsetLonBy = allPoints.Select(p => p.Lon).Min();
            var scaleLatBy = scaleLatTo / (allPoints.Select(p => p.Lat).Max() - offsetLatBy);
            var scaleLonBy = scaleLonTo / (allPoints.Select(p => p.Lon).Max() - offsetLonBy);

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