using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Redcat.Xmpp.Parsing
{
    public static class XmlLexer
    {
        public static readonly string XmlEnclosedTagRegex = @"\<\w[\s|\S]*\/\>";
        public static readonly string XmlStartTagRegex = @"\<\w+[\s|\S]*\>";
        public static readonly string XmlClosingTagRegex = @"\<\/\w+\>";
        public static readonly string XmlValueRegex = @"[^\<\>\/]+";
        public static readonly string XmlElementWithValueRegex = string.Format(@"({0}\b)|({1})|({2})", XmlStartTagRegex, XmlValueRegex, XmlClosingTagRegex);
        public static readonly string GenericXmlElementRegex = string.Format("({0})|({1})", XmlEnclosedTagRegex, XmlElementWithValueRegex);

        public static IEnumerable<XmlToken> GetTokens(string xmlFragment)
        {
            var tokens = Regex.Matches(xmlFragment, GenericXmlElementRegex);
            return tokens.Cast<Match>().Select(m => new XmlToken(m.Value, GetTokenType(m.Value)));
        }

        public static string GetTagName(XmlToken token)
        {
            var match = Regex.Match(token.Text, @"\<\w+").Value;
            return match.Substring(1, match.Length - 1);
        }

        private static XmlTokenType GetTokenType(string token)
        {
            if (Regex.IsMatch(token, XmlEnclosedTagRegex)) return XmlTokenType.EnclosedTag;
            if (Regex.IsMatch(token, XmlStartTagRegex)) return XmlTokenType.StartTag;
            if (Regex.IsMatch(token, XmlClosingTagRegex)) return XmlTokenType.ClosingTag;
            return XmlTokenType.Value;
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
        EnclosedTag,
        StartTag,
        ClosingTag,
        Value
    }
}
