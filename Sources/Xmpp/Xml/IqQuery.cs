using System;
using System.Xml;

namespace Redcat.Xmpp.Xml
{
    public class IqQuery : XmlElement
    {
        private string xmlns;

        public IqQuery(string xmlns) : base("query", xmlns)
        {
            if (string.IsNullOrEmpty(xmlns)) throw new ArgumentNullException("xmlns");
            this.xmlns = xmlns;
        }
    }
}
