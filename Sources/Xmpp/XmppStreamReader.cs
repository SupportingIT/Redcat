using Redcat.Xmpp.Parsing;
using Redcat.Xmpp.Xml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace Redcat.Xmpp
{
    public class XmppStreamReader
    {
        private ICollection<IElementParser> parsers;
        private Stream stream;
        private XmlReader xmlReader;

        public XmppStreamReader(Stream stream)
        {
            if (stream == null) throw new ArgumentNullException("stream");
            this.stream = stream;
            parsers = new List<IElementParser>();
            xmlReader = XmlReader.Create(stream);
        }

        public ICollection<IElementParser> Parsers
        {
            get { return parsers; }
        }

        public Stream Stream
        {
            get { return stream; }
        }

        public Element Read()
        {
            xmlReader.Read();
            var parser = GetInitializedParser();
            ParseAttributes(parser);
            ParseContent(parser);

            return parser.ParsedElement;
        }

        private void ParseContent(IElementParser parser)
        {
            while (xmlReader.Read())
            {                
                if (xmlReader.NodeType == XmlNodeType.Text) parser.SetNodeValue(xmlReader.Value);
                if (xmlReader.NodeType == XmlNodeType.Element)
                {
                    parser.StartNode(xmlReader.Name);
                    ParseAttributes(parser);                    
                }
                if (xmlReader.Name == "stream:stream") break;
                if (xmlReader.NodeType == XmlNodeType.EndElement)
                {
                    parser.EndNode();
                    break;
                }
            }
        }

        private IElementParser GetInitializedParser()
        {
            var parser = parsers.FirstOrDefault(p => p.CanParse(xmlReader.Name));
            if (parser == null) throw new InvalidOperationException("No parsers for element " + xmlReader.Name);
            parser.NewElement(xmlReader.Name);
            return parser;
        }

        private void ParseAttributes(IElementParser parser)
        {
            if (xmlReader.HasAttributes)
            {
                xmlReader.MoveToFirstAttribute();
                do parser.AddAttribute(xmlReader.Name, xmlReader.Value);
                while (xmlReader.MoveToNextAttribute());
                xmlReader.MoveToElement();
            }
        }

        public static XmppStreamReader CreateReader(Stream stream)
        {
            XmppStreamReader reader = new XmppStreamReader(stream);
            //reader.Parsers.Add(new StreamHeaderParser());
            return reader;
        }
    }
}
