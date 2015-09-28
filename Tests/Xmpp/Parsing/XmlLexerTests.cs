using NUnit.Framework;
using Redcat.Xmpp.Parsing;
using System.Linq;

namespace Redcat.Xmpp.Tests.Parsing
{
    [TestFixture]
    public class XmlLexerTests
    {
        private string[] enclosedTags = { "<element />", "<element attr1='val0' attr2='val' />" };
        private string[] elementsWithValue = { "<node>Value</node>", "<node attr1='val0' attr2='val'>Value</node>" };
        private string[] tagsForGetTagName = { @"<kitty />", @"<kitty>", @"<kitty attr1='val1' attr0='val0'>" };

        [Test]
        public void GetTokens_Correctly_Parses_Enclosed_Tag([ValueSource("enclosedTags")]string tag)
        {
            var tokens = XmlLexer.GetTokens(tag).ToArray();
            Assert.That(tokens.Length, Is.EqualTo(1));
            Assert.That(tokens[0].Type, Is.EqualTo(XmlTokenType.EnclosedTag));
        }

        [Test]
        public void GetTokens_Correctly_Parses_Element_With_Value([ValueSource("elementsWithValue")]string element)
        {
            var tokens = XmlLexer.GetTokens(element).ToArray();

            Assert.That(tokens.Length, Is.EqualTo(3));
            Assert.That(tokens[0].Type, Is.EqualTo(XmlTokenType.StartTag));
            Assert.That(tokens[1].Type, Is.EqualTo(XmlTokenType.Value));
            Assert.That(tokens[1].Text, Is.EqualTo("Value"));
            Assert.That(tokens[2].Type, Is.EqualTo(XmlTokenType.ClosingTag));
        }

        [Test]
        public void GetTagName_Returns_Tag_Name([ValueSource("tagsForGetTagName")]string tag)
        {
            XmlToken token = XmlLexer.GetTokens(tag).Single();

            string name = XmlLexer.GetTagName(token);

            Assert.That(name, Is.EqualTo("kitty"));
        }
    }
}
