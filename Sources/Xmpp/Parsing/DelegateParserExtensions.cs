using System;
using Redcat.Xmpp.Xml;

namespace Redcat.Xmpp.Parsing
{
    public static class DelegateParserExtensions
    {
        public static DelegateParser<T> Attribute<T>(this DelegateParser<T> parser, string name, Action<T, string> assignFunc) where T : XmlElement
        {
            parser.AttributeBuilders[name] = context => assignFunc(context.Element, context.AttributeValue);
            return parser;
        }

        public static DelegateParser<T> NodeValue<T>(this DelegateParser<T> parser, string nodeName, Action<T, string> assignFunc) where T : XmlElement
        {
            parser.AddNodeBuilder(nodeName, context => assignFunc(context.Element, context.NodeValue));
            return parser;
        }
    }
}
