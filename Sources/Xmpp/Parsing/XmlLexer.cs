using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Redcat.Xmpp.Parsing
{
    public static class XmlLexer
    {
        private static readonly string XmlEnclosedTagRegex = @"\<[\w-\:]+[^\<\>\/]*\/\>";
        private static readonly string XmlStartTagRegex = @"\<[\w-\:]+[^\<\>\/]*\>";
        private static readonly string XmlClosingTagRegex = @"\<\/\s*\w+\>";
        private static readonly string XmlValueRegex = @"[^\<\>\/]+";
        private static readonly string WhitespaceRegex = @"[\s\t]+";
        private static readonly string GenericXmlElementRegex = string.Format(@"({0})|({1})|({2})|({3})", XmlStartTagRegex, XmlClosingTagRegex, XmlEnclosedTagRegex, XmlValueRegex);

        private static readonly string XmlAttributeRegex = @"[\w-\:]+=('|"")[\w-\:]*\1";

        public static IEnumerable<XmlToken> GetTokens(string xmlFragment)
        {
            var tokens = Regex.Matches(xmlFragment, GenericXmlElementRegex);
            return tokens.Cast<Match>().Select(m => new XmlToken(m.Value, GetTokenType(m.Value)));
        }

        public static IEnumerable<Tuple<string, string>> GetTagAttributes(XmlToken token)
        {
            var attributes = Regex.Matches(token.Text, XmlAttributeRegex).Cast<Match>().Select(m => m.Value).ToArray();
            return attributes.Select(a =>
            {
                var attr = a.Split('=');
                return new Tuple<string, string>(attr[0], Regex.Match(attr[1], @"[^""']+").Value);
            });
        }

        public static string GetTagName(XmlToken token)
        {
            return Regex.Match(token.Text, @"[\w-\:]+").Value;
        }

        public static XmlTokenType GetTokenType(string token)
        {
            if (IsEnclosedTag(token)) return XmlTokenType.EnclosedTag;
            if (IsStartTag(token)) return XmlTokenType.StartTag;
            if (IsClosingTag(token)) return XmlTokenType.ClosingTag;
            if (IsWhitespace(token)) return XmlTokenType.Whitespace;
            return XmlTokenType.Value;
        }

        public static bool IsEnclosedTag(string token)
        {
            return Regex.IsMatch(token, XmlEnclosedTagRegex);
        }

        public static bool IsStartTag(string token)
        {
            return Regex.IsMatch(token, XmlStartTagRegex);
        }

        public static bool IsClosingTag(string token)
        {
            return Regex.IsMatch(token, XmlClosingTagRegex);
        }

        public static bool IsWhitespace(string token)
        {
            return Regex.IsMatch(token, WhitespaceRegex);
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

        public override string ToString()
        {
            return string.Format("{0}[{1}]", Text, Type);
        }
    }

    public enum XmlTokenType
    {
        EnclosedTag,
        StartTag,
        ClosingTag,
        Value,
        Whitespace
    }
}
