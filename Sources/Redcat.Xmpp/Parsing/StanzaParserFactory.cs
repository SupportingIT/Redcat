using System;
using Redcat.Xmpp.Xml;

namespace Redcat.Xmpp.Parsing
{
    public static class StanzaParserFactory
    {
        public static DelegateParser<T> CreateDatagramParser<T>(Func<string, T> createElementFunc, params string[] supportedElements) where T : DatagramElement
        {
            var parser = new DelegateParser<T>(createElementFunc, supportedElements);
            return parser.Attribute("id", (e, v) => e.Id = v)
                         .Attribute("from", (e, v) => e.From = v)
                         .Attribute("to", (e, v) => e.To = v);
        }

        public static DelegateParser<T> CreateStanzaParser<T>(Func<string, T> createElementFunc, params string[] supportedElements) where T : Stanza
        {
            return CreateDatagramParser(createElementFunc, supportedElements)
                .Attribute("type", (e, v) => e.Type = v);
        }

        public static IElementParser CreatePresenceParser()
        {
            return CreateStanzaParser(s => new PresenceStanza(), "presence")
                .NodeValue("show", (s, v) => s.Show = v)
                .NodeValue("status", (s, v) => s.Status = v);
        }

        public static IElementParser CreateMessageParser()
        {
            return CreateStanzaParser(s => new MessageStanza(), "message")
                .NodeValue("subject", (m, v) => m.Subject = v)
                .NodeValue("body", (m, v) => m.Body = v);
        }
    }
}
