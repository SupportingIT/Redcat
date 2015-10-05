using Redcat.Xmpp.Parsing;
using Redcat.Xmpp.Xml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Redcat.Xmpp
{
    public class XmppStreamReader
    {
        private static readonly Encoding defaultEncoding = Encoding.UTF8;
        private IXmlParser parser;
        private TextReader reader;
        private Queue<XmlElement> elementQueue = new Queue<XmlElement>();

        public XmppStreamReader(Stream stream)
        {
            if (stream == null) throw new ArgumentNullException("stream");
            reader = new StreamReader(stream, defaultEncoding);
        }

        public XmppStreamReader(TextReader reader)
        {
            if (reader == null) throw new ArgumentNullException();
            this.reader = reader;
        }

        public IXmlParser Parser
        {
            get { return parser ?? (parser = CreateDefaultParser()); }
            set { parser = value; }
        }

        private IXmlParser CreateDefaultParser()
        {
            return new XmppStreamParser();
        }

        public XmlElement Read()
        {
            string xml = reader.ReadToEnd();
            foreach (var element in Parser.Parse(xml))
            {
                elementQueue.Enqueue(element);
            }

            if (elementQueue.Count == 0) return null;
            return elementQueue.Dequeue();
        }
    }
}
