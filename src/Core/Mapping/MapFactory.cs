using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;

namespace StreetRunner.Core.Mapping
{
    public static class MapFactory
    {
        public static Map FromOsm(string osd)
        {
            return FromOsm(osd, Enumerable.Empty<IRun>());
        }

        public static Map FromOsm(string osd, IEnumerable<IRun> runs)
        {
            return FromOsm(osd, runs, new JObject());
        }
        
        public static Map FromOsm(string osd, IEnumerable<IRun> runs, JObject mapJson)
        {
            var osdXml = XElement.Parse(osd);

            // this lookup was very slow, scanning through just once now
            var nodeDict = osdXml
                .Elements("node")
                .ToDictionary(node => node.Attribute("id").Value, node => node);

            var streets = osdXml.Elements("way")
                .Where(way => way.Elements("tag").Any(tag => tag.Attribute("k").Value == "highway"))
                .Select(way => 
                {
                    var name = String.Empty;
                    if (way.Elements("tag").Any(tag => tag.Attribute("k").Value == "name")) 
                    {
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
                            var match = nodeDict[id];
                            var lat = Decimal.Parse(match.Attribute("lat").Value);
                            var lon = Decimal.Parse(match.Attribute("lon").Value);
                            return new Point(lat, lon);
                        })
                        .ToList();
                    

                    return new Street(name, points, type);
                })
                .ToList();

            return new Map(streets, runs, mapJson);
        }
    }
}