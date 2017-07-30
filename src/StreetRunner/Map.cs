using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace StreetRunner
{
    public class Map
    {
        public IEnumerable<Street> Streets {get;set;}
        
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
            return Streets.Single().ToSvgPath(scaleLatTo, scaleLonTo);
        }

        public Map(IEnumerable<Street> streets)
        {
            Streets = streets;
        }
    }
}