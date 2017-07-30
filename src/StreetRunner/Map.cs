using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace StreetRunner
{
    public class Map
    {
        private IEnumerable<Street> _streets;

        public IEnumerable<Street> Streets => _streets
            // .Where(s => s.Name == "Alie Street" || s.Name == "Prescot Street" || s.Name == "Leman Street")
            ;
        
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
            var allPoints = Streets.SelectMany(s => s.Points);

            var offsetLatBy = allPoints.Select(p => p.Lat).Min();
            var offsetLonBy = allPoints.Select(p => p.Lon).Min();
            var scaleLatBy = scaleLatTo / (allPoints.Select(p => p.Lat).Max() - offsetLatBy);
            var scaleLonBy = scaleLonTo / (allPoints.Select(p => p.Lon).Max() - offsetLonBy);

            var paths = Streets.Select(street => {
                return ToSvgPath(scaleLonTo, offsetLatBy, offsetLonBy, scaleLatBy, scaleLonBy, street);
            });
            
            return String.Join(Environment.NewLine, paths);
        }

        private static string ToSvgPath(int scaleLonTo, decimal offsetLatBy, decimal offsetLonBy, decimal scaleLatBy, decimal scaleLonBy, Street street)
        {
            var scaledPoints = street.Points
                            .Select(p => new Point(p.Lat - offsetLatBy, p.Lon - offsetLonBy))
                            .Select(p => new Point(p.Lat * scaleLatBy, Math.Abs((int)(p.Lon * scaleLonBy - scaleLonTo))));

            var first = scaledPoints.First();

            var path = string.Join("", scaledPoints
                .Select(p => $"L {p.Lon} {p.Lat} ")
                ).Substring(1);

            return $"<path d=\"M{path}\" stroke=\"black\" fill=\"transparent\"/>";
        }

        public Map(IEnumerable<Street> streets)
        {
            _streets = streets;
        }
    }
}