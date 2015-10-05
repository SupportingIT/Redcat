using System.Collections.Generic;
using Redcat.Xmpp.Xml;

namespace Redcat.Xmpp.Parsing
{
    public interface IXmlParser
    {
        IEnumerable<XmlElement> Parse(string xmlText);
    }
}
