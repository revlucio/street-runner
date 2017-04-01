using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace tests
{
    internal class Map
    {
        public IEnumerable<Street> Streets {get;set;}
        
        internal static Map FromOsd(string osd)
        {
            var osdXml = XElement.Parse(osd);
            var streets = osdXml.Elements("way")
                .Select(way => {
                    var nameTag = way.Elements("tag").Single(tag => tag.Attribute("k").Value == "name");
                    return new Street(nameTag.Attribute("v").Value);
                });

            return new Map(streets);
        }

        public Map(IEnumerable<Street> streets)
        {
            Streets = streets;
        }
    }
}