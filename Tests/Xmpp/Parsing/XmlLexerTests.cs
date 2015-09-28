using NUnit.Framework;
using Redcat.Xmpp.Parsing;
using System.Linq;

namespace Redcat.Xmpp.Tests.Parsing
{
    [TestFixture]
    public class XmlLexerTests
    {
        [Test]
        public void GetTokens_Correctly_Parses_Enclosed_Tag()
        {
            VerifyEnclosingTagParsing("<element />");
        }

        [Test]
        public void GetTokens_Correctly_Parses_Enclosed_Tag_With_Attributes()
        {
            VerifyEnclosingTagParsing("<element attr1='val0' attr2='val' />");
        }

        private void VerifyEnclosingTagParsing(string tag)
        {
            var tokens = XmlLexer.GetTokens(tag).ToArray();
            Assert.That(tokens.Length, Is.EqualTo(1));
            Assert.That(tokens[0].Type, Is.EqualTo(XmlTokenType.EnclosedTag));
        }

        [Test]
        public void GetTokens_Correctly_Parses_Element_With_Value()
        {
            VerifyElementWithValueParsing("<node>Value</node>");
        }

        [Test]
        public void GetTokens_Correctly_Parses_Element_With_Attributes_And_Value()
        {
            VerifyElementWithValueParsing("<node attr1='val0' attr2='val'>Value</node>");
        }

        private void VerifyElementWithValueParsing(string element)
        {
            var tokens = XmlLexer.GetTokens(element).ToArray();

            Assert.That(tokens.Length, Is.EqualTo(3));
            Assert.That(tokens[0].Type, Is.EqualTo(XmlTokenType.StartTag));
            Assert.That(tokens[1].Type, Is.EqualTo(XmlTokenType.Value));
            Assert.That(tokens[1].Text, Is.EqualTo("Value"));
            Assert.That(tokens[2].Type, Is.EqualTo(XmlTokenType.ClosingTag));
        }

        [Test]
        public void GetTagName_Returns_Tag_Name([Values(@"<kitty />", @"<kitty>", @"<kitty attr1='val1' attr0='val0'>")]string tag)
        {
            XmlToken token = XmlLexer.GetTokens(tag).Single();

            string name = XmlLexer.GetTagName(token);

            Assert.That(name, Is.EqualTo("kitty"));
        }
    }
}
