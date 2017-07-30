using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace StreetRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to STREET RUNNER");
            var timer = Stopwatch.StartNew();

            var map = OutputAlieStreet();

            Console.WriteLine("Took: " + timer.Elapsed.ToString());
            Console.WriteLine(map.Streets.Count() + " streets found");
        }

        private static Map OutputAlieStreet()
        {
            var osd = File.ReadAllText(Directory.GetCurrentDirectory() + "/east-london.osm");
            var map = Map.FromOsd(osd);
            Console.WriteLine(map.ToSvgPath(500, 500));
            return map;
        }
    }
}
