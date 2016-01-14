using Redcat.Xmpp.Xml;
using System;
using System.IO;
using System.Threading.Tasks;

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

        public async Task<XmlElement> ReadAsync()
        {
            return await reader.ReadAsync();
        }

        public void Write(XmlElement element)
        {
            writer.Write(element);
        }

        public async Task WriteAsync(XmlElement element)
        {
            throw new NotImplementedException();
        }
    }
}
