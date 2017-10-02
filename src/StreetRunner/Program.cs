using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using StreetRunner.Mapping;
using StreetRunner.Xml;

namespace StreetRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            StripDownOsd();
            // RunStreetRunner();
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
            var streetsToInclude = new List<string> {
                "Alie Street",
                "Prescot Street",
                "Leman Street",
            };

            var osd = File.ReadAllText(Directory.GetCurrentDirectory() + "/east-london.osm");
            var xml = XElement.Parse(osd);

            xml.GetElements("way")
                .Where(way =>
                {
                    return !way.Elements().Any(e =>
                    {
                        return e.Name.LocalName == "tag" && streetsToInclude.Contains(e.Attribute("v").Value);
                    });
                })
                .Remove();

            xml.GetElements("way")
                .Take(5000)
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

            File.WriteAllText("small-east-london.osm", xml.ToString());
        }

        private static Map OutputAlieStreet()
        {
            var osd = File.ReadAllText(Directory.GetCurrentDirectory() + "/east-london.osm");
            var gpx = File.ReadAllText(Directory.GetCurrentDirectory() + "/east-london.gpx");
            
            var map = MapFactory.FromOsd(osd);
            map.AddRun(gpx);
            Console.WriteLine(map.ToSvg(500, 500));
            return map;
        }
    }
}
