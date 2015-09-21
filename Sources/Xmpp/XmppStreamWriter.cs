using Redcat.Xmpp.Xml;
using System.IO;
using System.Xml;

namespace Redcat.Xmpp
{
    public class XmppStreamWriter
    {
        private Stream stream;
        private XmlWriter xmlWriter;

        public XmppStreamWriter(Stream stream)
        {
            this.stream = stream;
            XmlWriterSettings settings = new XmlWriterSettings
            {
                OmitXmlDeclaration = true
            };
            xmlWriter = XmlWriter.Create(stream, settings);
        }

        public void Write(Element element)
        {
            element.Write(xmlWriter);
            xmlWriter.Flush();
        }
    }
}
