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
        private TextReader reader;

        private XmppStreamReader()
        {
            builders = new List<IXmlElementBuilder>();
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
            var tokens = ReadXmlTokens();
            var builder = GetBuilder(tokens[0]);
            BuildElement(builder, tokens);
            return builder.Element;
        }

        private XmlToken[] ReadXmlTokens()
        {
            string xml = reader.ReadToEnd();
            return XmlLexer.GetTokens(xml).ToArray();
        }

        private IXmlElementBuilder GetBuilder(XmlToken token)
        {
            string name = XmlLexer.GetTagName(token);
            var builder = builders.FirstOrDefault(b => b.CanBuild(name));
            if (builder == null) throw new InvalidOperationException("No builders for element " + name);
            return builder;
        }

        private void BuildElement(IXmlElementBuilder builder, XmlToken[] tokens)
        {
            string elementName = XmlLexer.GetTagName(tokens[0]);
            builder.NewElement(elementName);
            BuildAttributes(builder, tokens[0]);
            BuildElementContent(builder, tokens);
        }

        private void BuildElementContent(IXmlElementBuilder builder, XmlToken[] tokens)
        {
            foreach (var token in tokens.Skip(1))
            {
                if (IsStartOrEnclosedTag(token)) BuildStartNode(builder, token);
                if (IsValue(token)) builder.SetNodeValue(token.Text);
                if (IsClosingOrEnclosedTag(token)) builder.EndNode();
            }
        }

        private bool IsValue(XmlToken token)
        {
            return token.Type == XmlTokenType.Value;
        }

        private bool IsClosingOrEnclosedTag(XmlToken token)
        {
            return token.Type == XmlTokenType.ClosingTag || token.Type == XmlTokenType.EnclosedTag;
        }

        private void BuildStartNode(IXmlElementBuilder builder, XmlToken token)
        {
            string name = XmlLexer.GetTagName(token);
            builder.StartNode(name);
            BuildAttributes(builder, token);
        }

        private void BuildAttributes(IXmlElementBuilder builder, XmlToken token)
        {
            if (!IsStartOrEnclosedTag(token)) return;

            var attributes = XmlLexer.GetTagAttributes(token);
            foreach (var attribute in attributes)
            {
                builder.AddAttribute(attribute.Item1, attribute.Item2);
            }
        }

        private bool IsStartOrEnclosedTag(XmlToken token)
        {
            return token.Type == XmlTokenType.StartTag || token.Type == XmlTokenType.EnclosedTag;
        }

        public static XmppStreamReader CreateReader(Stream stream)
        {
            XmppStreamReader reader = new XmppStreamReader(stream);
            //reader.Parsers.Add(new StreamHeaderParser());
            return reader;
        }
    }
}
