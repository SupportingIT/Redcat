using Redcat.Xmpp.Xml;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Redcat.Xmpp.Parsing
{
    public class XmppStreamParser : IXmlParser
    {
        private IXmlElementBuilder builder;
        private XmlLexer lexer;            

        public XmppStreamParser()
        {
            builder = new XmlElementBuilder();
            lexer = new XmlLexer();
            lexer.Options.ParseTagName = true;
        }

        public IEnumerable<XmlElement> Parse(string xmlText)
        {
            if (xmlText == null) throw new ArgumentNullException("xmlText");
            if (xmlText == "") return Enumerable.Empty<XmlElement>();
            XmlToken[] tokens = lexer.GetTokens(xmlText);
            return ParseTokens(tokens);
        }

        private IEnumerable<XmlElement> ParseTokens(XmlToken[] tokens)
        {
            List<XmlElement> parsedElements = new List<XmlElement>();

            for (int i = 0; i < tokens.Length; i++)
            {
                if (tokens[i].Type == XmlTokenType.EnclosedTag) BuildSingleTagElement(builder, tokens[i]);
                if (tokens[i].Type == XmlTokenType.StartTag)
                {
                    if (tokens[i].TagName != "stream:stream")
                    {
                        int index = GetClosingTagIndex(tokens, tokens[i].TagName, i);
                        BuildMultiTagElement(tokens, i, index);
                        i = index;
                    }
                    else BuildSingleTagElement(builder, tokens[i]);
                }
                if (builder.Element != null) parsedElements.Add(builder.Element);
            }

            return parsedElements;
        }

        private int GetClosingTagIndex(XmlToken[] tokens, string name, int offset)
        {
            for (int i = offset; i < tokens.Length; i++)
            {
                if (tokens[i].TagName == name && tokens[i].Type == XmlTokenType.ClosingTag) return i;
            }
            throw new NotImplementedException();
        }

        private void BuildSingleTagElement(IXmlElementBuilder builder, XmlToken token)
        {
            builder.NewElement(token.TagName);
            BuildAttributes(builder, token);
        }

        private void BuildMultiTagElement(XmlToken[] tokens, int offset, int count)
        {
            builder.NewElement(tokens[offset].TagName);
            BuildAttributes(builder, tokens[offset]);
            
            for (int i = offset + 1; i < count; i++)
            {
                if (tokens[i].Type == XmlTokenType.Value) builder.SetNodeValue(tokens[i].Value);
                if (tokens[i].Type == XmlTokenType.EnclosedTag) BuildChildEnclosedElement(builder, tokens[i]);                
                if (tokens[i].Type == XmlTokenType.StartTag) builder.StartNode(tokens[i].TagName);
                if (tokens[i].Type == XmlTokenType.ClosingTag) builder.EndNode();
            }
        }

        private void BuildChildEnclosedElement(IXmlElementBuilder builder, XmlToken token)
        {
            builder.StartNode(token.TagName);
            BuildAttributes(builder, token);
            builder.EndNode();
        }

        private void BuildAttributes(IXmlElementBuilder builder, XmlToken token)
        {
            var attributes = XmlLexer.GetTagAttributes(token);
            foreach (var attribute in attributes)
            {
                builder.AddAttribute(attribute.Item1, attribute.Item2);
            }
        }
    }
}
