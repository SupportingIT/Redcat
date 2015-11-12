using Redcat.Xmpp.Xml;
using System.IO;

namespace Redcat.Xmpp
{
    public class XmppStream : IXmppStream
    {
        private XmppStreamReader reader;
        private XmppStreamWriter writer;

        public XmppStream(Stream stream)
        {
            reader = new XmppStreamReader(stream);
            writer = new XmppStreamWriter(stream);
        }

        public XmlElement Read()
        {
            return reader.Read();
        }

        public void Write(XmlElement element)
        {
            writer.Write(element);
        }
    }
}
