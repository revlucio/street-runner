using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace StreetRunner
{
    public class Run
    {
        private string runName;

        private Run(string runName, IEnumerable<Point> points)
        {
            this.runName = runName;
            this.Points = points;
        }

        public string Name => runName;
        public IEnumerable<Point> Points { get; }

        internal static Run FromGpx(string gpx)
        {
            var gpxXml = XElement.Parse(gpx);
            var runXml = gpxXml.GetElement("trk");
            var name = runXml.GetElement("name").Value;
            IEnumerable<Point> points = GetPoints(runXml);

            return new Run(name, points);
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