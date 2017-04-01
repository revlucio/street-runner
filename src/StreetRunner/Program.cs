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
            var osd = File.ReadAllText(Directory.GetCurrentDirectory() + "/east-london.osm");
            var map = Map.FromOsd(osd);
            var streets = map.Streets.Where(s => s.Name == "Alie Street").ToList();
            
            streets.ForEach(street => 
            {
                var offsetLatBy = 51.5147m;
                var scaleLatBy = 500 / (51.513m - offsetLatBy);
                var offsetLonBy = -0.073m;
                var scaleLonBy = 500 / (-0.069m - offsetLonBy);
                Console.WriteLine(street.ToSvgPath(offsetLatBy, offsetLonBy, scaleLatBy, scaleLonBy));
            });

            Console.WriteLine("Took: " + timer.Elapsed.ToString());
            Console.WriteLine(streets.Count() + " streets found");
        }
    }
}
