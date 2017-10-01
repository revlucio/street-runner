using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace StreetRunner
{
    public static class XElementExtensions
    {
        public static XElement GetElement(this XElement xml, string name)
        {
            return xml.Elements().Single(e => e.Name.LocalName == name);
        }

        public static IEnumerable<XElement> GetElements(this XElement xml, string name)
        {
            return xml.Elements().Where(e => e.Name.LocalName == name);
        }
    }
}