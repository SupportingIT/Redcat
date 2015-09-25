using System;
using Redcat.Xmpp.Xml;
using System.IO;
using System.Text;

namespace Redcat.Xmpp
{
    public class XmppStreamWriter
    {
        private static Encoding defaultEncoding = Encoding.UTF8;
        private TextWriter writer;

        public XmppStreamWriter(Stream stream)
        {
            if (stream == null) throw new ArgumentNullException("stream");
            writer = new StreamWriter(stream, defaultEncoding);
        }

        public XmppStreamWriter(TextWriter writer)
        {
            if (writer == null) throw new ArgumentNullException("writer");
            this.writer = writer;
        }

        public void Write(XmlElement element)
        {
            writer.Write('<');
            writer.Write(element.Name);

            foreach (var attribute in element.Attributes)
            {
                writer.Write(" ");
                writer.Write(attribute.Name);
                writer.Write("='");
                writer.Write(attribute.Value);
                writer.Write("'");
            }

            if (element.Value == null)
            {
                writer.Write(" />");
                return;
            }

            writer.Write(">");
            writer.Write(element.Value);
            writer.Write("</");
            writer.Write(element.Name);
            writer.Write(">");
        }
    }
}
