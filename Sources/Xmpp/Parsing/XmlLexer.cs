using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Redcat.Xmpp.Parsing
{
    public static class XmlLexer
    {
        public static readonly string XmlTagRegex = @"\<\w[\s|\S]*/\>";
        public static readonly string XmlStartTagRegex = @"\<\w+\>";
        public static readonly string XmlClosingTagRegex = @"\</\w+\>";
        public static readonly string XmlValueRegExp = @"[^\<\>/]+";
        public static readonly string XmlNodeRegExp = string.Format(@"({0})|([^\<\>/]+)|({1})", XmlStartTagRegex, XmlClosingTagRegex);

        public static IEnumerable<XmlToken> GetTokens(string xmlFragment)
        {
            var tokens = Regex.Matches(xmlFragment, XmlNodeRegExp);
            return tokens.Cast<Match>().Select(m => new XmlToken(m.Value, GetTokenType(m.Value)));
        }

        private static XmlTokenType GetTokenType(string token)
        {
            if (Regex.IsMatch(token, XmlStartTagRegex)) return XmlTokenType.StartTag;
            if (Regex.IsMatch(token, XmlValueRegExp)) return XmlTokenType.Value;
            if (Regex.IsMatch(token, XmlClosingTagRegex)) return XmlTokenType.ClosingTag;
            return XmlTokenType.Tag;
        }
    }

    public class XmlToken
    {
        private XmlTokenType type;
        private string text;

        internal XmlToken(string text, XmlTokenType type)
        {
            this.type = type;
            this.text = text;
        }

        public string Text
        {
            get { return text; }
        }

        public XmlTokenType  Type
        {
            get { return type; }
        }
    }

    public enum XmlTokenType
    {
        StartTag,
        ClosingTag,
        Tag,
        Value
    }
}
