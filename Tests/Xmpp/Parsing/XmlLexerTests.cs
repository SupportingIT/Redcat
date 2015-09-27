using NUnit.Framework;
using Redcat.Xmpp.Parsing;
using System.Linq;

namespace Redcat.Xmpp.Tests.Parsing
{
    [TestFixture]
    public class XmlLexerTests
    {
        [Test]
        public void GetTokens_Correctly_Parses_Single_Tag()
        {
            string element = "<element />";

            var tokens = XmlLexer.GetTokens(element);

            Assert.That(tokens.Count(), Is.EqualTo(1));
            Assert.That(tokens.ElementAt(0).Type, Is.EqualTo(XmlTokenType.Tag));
        }

        [Test]
        public void GetTokens_Correctly_Parses_Element_With_Value()
        {
            string element = "<node>Value</node>";

            var tokens = XmlLexer.GetTokens(element).ToArray();

            //Assert.That(tokens.Length, Is.EqualTo(3));
            //Assert.That(tokens[0].Type, Is.EqualTo(XmlTokenType.StartTag));
            //Assert.That(tokens[1].Type, Is.EqualTo(XmlTokenType.Value));
            //Assert.That(tokens[1].Text, Is.EqualTo("Value"));
            Assert.That(tokens[2].Type, Is.EqualTo(XmlTokenType.ClosingTag));
        }
    }
}
