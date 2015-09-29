using System;
using System.Collections.Generic;
using NUnit.Framework;
using Redcat.Xmpp.Parsing;
using System.Linq;

namespace Redcat.Xmpp.Tests.Parsing
{
    [TestFixture]
    public class XmlLexerTests
    {
        private string[] enclosedTags = { "<element />", "<element attr1='val0' attr2='val' />" };
        
        [Test]
        public void GetTokens_Correctly_Parses_Enclosed_Tag([ValueSource("enclosedTags")]string tag)
        {
            var tokens = XmlLexer.GetTokens(tag).ToArray();
            Assert.That(tokens.Length, Is.EqualTo(1));
            Assert.That(tokens[0].Type, Is.EqualTo(XmlTokenType.EnclosedTag));
        }

        private string[] elementsWithValue = { "<node>Value</node>", "<node attr1='val0' attr2='val'>Value</node>" };

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

        private Tuple<string, string>[] dataForGetTagName =
        {
            new Tuple<string, string>(@"<kitty />", "kitty"), 
            new Tuple<string, string>(@"<k:itty>", "k:itty"),
            new Tuple<string, string>(@"<kitt-y8 attr1='val1' attr0='val0'>", "kitt-y8") 
        };

        [Test]
        public void GetTagName_Returns_Tag_Name([ValueSource("dataForGetTagName")]Tuple<string, string> data)
        {
            XmlToken token = XmlLexer.GetTokens(data.Item1).Single();

            string name = XmlLexer.GetTagName(token);

            Assert.That(name, Is.EqualTo(data.Item2));
        }

        [Test]
        public void GetTagAttributes_Returns_Empty_Collection_For_Tag_Without_Attributes()
        {
            XmlToken token = XmlLexer.GetTokens("<someone >").Single();

            var attributes = XmlLexer.GetTagAttributes(token);

            Assert.That(attributes.Count(), Is.EqualTo(0));
        }

        [Test]
        public void GetTagAttributes_Returns_Correct_Collection_Of_Attributes()
        {
            string tag = "<tag attr1='val1' attr-2='8' d:tr='12-1' void=''>";
            var expectedAttributes = new List<Tuple<string, string>>
            {
                new Tuple<string, string>("attr1", "val1"),
                new Tuple<string, string>("attr-2", "8"),
                new Tuple<string, string>("d:tr", "12-1"),
                new Tuple<string, string>("void", "")
            };
            var token = XmlLexer.GetTokens(tag).Single();

            var actualAttributes = XmlLexer.GetTagAttributes(token).ToArray();

            Assert.That(actualAttributes, Is.EquivalentTo(expectedAttributes));
        }
    }
}
