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
        private static Encoding defaultEncoding = Encoding.UTF8;
        private ICollection<IXmlElementBuilder> builders;
        private IXmlElementBuilder defaultBuilder;
        private TextReader reader;

        private XmppStreamReader()
        {
            builders = new List<IXmlElementBuilder>();
            defaultBuilder = new XmlElementBuilder();
        }

        public XmppStreamReader(Stream stream) : this()
        {
            if (stream == null) throw new ArgumentNullException("stream");
            reader = new StreamReader(stream, defaultEncoding);
        }

        public XmppStreamReader(TextReader reader) : this()
        {
            if (reader == null) throw new ArgumentNullException();
            this.reader = reader;
        }

        public ICollection<IXmlElementBuilder> Builders
        {
            get { return builders; }
        }

        public XmlElement Read()
        {
            throw new NotImplementedException();
        }       

        public static XmppStreamReader CreateReader(Stream stream)
        {
            XmppStreamReader reader = new XmppStreamReader(stream);
            //reader.Parsers.Add(new StreamHeaderParser());
            return reader;
        }
    }
}
