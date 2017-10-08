using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using StreetRunner.Core.Xml;

namespace StreetRunner.Core.Mapping
{
    public class Run : IRun
    {
        public Run(string runName, IEnumerable<Point> points)
        {
            Name = runName;
            Points = points;
        }

        public string Name { get; }

        public IEnumerable<Point> Points { get; }

        public static Run FromGpx(string gpx)
        {
            var gpxXml = XElement.Parse(gpx);
            var runXml = gpxXml.GetElement("trk");
            var name = runXml.GetElement("name").Value;
            var points = GetPoints(runXml);

            return new Run(name, points.ToList());
        }

        private static IEnumerable<Point> GetPoints(XElement runXml)
        {
            if (runXml.Elements().Any(e => e.Name.LocalName == "trkseg") == false) 
            {
                return Enumerable.Empty<Point>();
            }

            return runXml
                .GetElement("trkseg")
                .GetElements("trkpt")
                .Select(p => new Point(decimal.Parse(p.Attribute("lat").Value), decimal.Parse(p.Attribute("lon").Value)));
        }
    }
}