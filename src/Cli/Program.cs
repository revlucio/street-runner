using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using StreetRunner.Core.Mapping;
using StreetRunner.Core.Xml;

namespace StreetRunner.Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            StripDownOsd();
        }

        private static void RunStreetRunner()
        {
            Console.WriteLine("Welcome to STREET RUNNER");
            var timer = Stopwatch.StartNew();

            var map = OutputAlieStreet();

            Console.WriteLine("Took: " + timer.Elapsed.ToString());
            Console.WriteLine(map.Streets.Count() + " streets found");
        }

        private static void StripDownOsd()
        {
            var osd = File.ReadAllText(Directory.GetCurrentDirectory() + "/map.osm");
            var xml = XElement.Parse(osd);

            xml.GetElements("way")
                .Where(way =>
                {
                    return !way.Elements().Any(e =>
                    {
                        return e.Name.LocalName == "tag";
                    });
                })
                .Remove();

            xml.GetElements("relation").Remove();

            var nodeIds = xml.GetElements("way")
                .SelectMany(way => way.GetElements("nd"))
                .Select(nd => nd.Attribute("ref").Value)
                .Distinct()
                .ToList();

            xml.GetElements("node")
                .Where(node => !nodeIds.Contains(node.Attribute("id").Value))
                .Remove();

            File.WriteAllText("small-map.osm", xml.ToString());
        }

        private static Map OutputAlieStreet()
        {
            var osd = File.ReadAllText(Directory.GetCurrentDirectory() + "/east-london.osm");
            var gpx = File.ReadAllText(Directory.GetCurrentDirectory() + "/east-london.gpx");
            
            var run = Run.FromGpx(gpx);
            var map = MapFactory.FromOsm(osd, new [] {run});
            Console.WriteLine(map.ToSvg(500, 500));
            return map;
        }
    }
}
