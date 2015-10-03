using Redcat.Xmpp.Xml;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Redcat.Xmpp.Parsing
{
    public class XmppStreamParser
    {
        private IXmlElementBuilder builder;
        private XmlLexer lexer;            

        public XmppStreamParser()
        {
            builder = new XmlElementBuilder();
            lexer = new XmlLexer();
            lexer.Options.ParseTagName = true;
        }

        public IEnumerable<XmlElement> Parse(string xml)
        {
            if (xml == "") return Enumerable.Empty<XmlElement>();
            XmlToken[] tokens = lexer.GetTokens(xml);
            List<XmlElement> parsedElements = new List<XmlElement>();

            for (int i = 0; i < tokens.Length; i++)
            {
                if (tokens[i].Type == XmlTokenType.EnclosedTag) BuildSingleTagElement(tokens[i]);                
                if (tokens[i].Type == XmlTokenType.StartTag)
                {                    
                    BuildMultiTagElement(tokens, i);
                }
                parsedElements.Add(builder.Element);
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

        private void BuildSingleTagElement(XmlToken token)
        {
            string name = XmlLexer.GetTagName(token);
            builder.NewElement(name);
            var attributes = XmlLexer.GetTagAttributes(token);
            foreach (var attribute in attributes) builder.AddAttribute(attribute.Item1, attribute.Item2);
        }

        private void BuildMultiTagElement(XmlToken[] tokens, int offset)
        {
            builder.NewElement(tokens[offset].TagName);
            int count = GetClosingTagIndex(tokens, tokens[offset].TagName, offset);
            for (int i = offset + 1; i < offset + count; i++)
            {
                if (tokens[i].Type == XmlTokenType.Value) builder.SetNodeValue(tokens[i].Value);
            }
        }
    }
}
