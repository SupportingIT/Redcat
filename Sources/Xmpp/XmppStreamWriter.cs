using Redcat.Xmpp.Xml;
using System.IO;
using System.Text;
using System.Xml;

namespace Redcat.Xmpp
{
    public class XmppStreamWriter
    {
        private static Encoding encoding = Encoding.UTF8;
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

        public void Write(string data)
        {
            byte[] binaryData = encoding.GetBytes(data);
            Write(binaryData);
        }

        public void Write(byte[] buffer)
        {
            stream.Write(buffer, 0, buffer.Length);
        }
    }
}
