using System;
using Redcat.Xmpp.Xml;

namespace Redcat.Xmpp.Parsing
{
    public static class DelegateParserExtensions
    {
        public static BuilderParser<T> Attribute<T>(this BuilderParser<T> parser, string name, Action<T, string> assignFunc) where T : XmlElement
        {
            parser.AttributeBuilders[name] = context => assignFunc(context.Element, context.AttributeValue);
            return parser;
        }

        public static BuilderParser<T> NodeValue<T>(this BuilderParser<T> parser, string nodeName, Action<T, string> assignFunc) where T : XmlElement
        {
            parser.AddNodeBuilder(nodeName, context => assignFunc(context.Element, context.NodeValue));
            return parser;
        }
    }
}
