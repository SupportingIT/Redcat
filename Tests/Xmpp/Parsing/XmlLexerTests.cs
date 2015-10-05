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
        private XmlLexer lexer;

        [SetUp]
        public void Setup()
        {
            lexer = new XmlLexer();
        }

        #region GetTokens method tests

        [Test]
        public void GetTokens_Correctly_Parses_Xml_Declaration()
        {
            string declaration = "<?xml version='1.0'?>";

            var token = lexer.GetTokens(declaration).Single();

            Assert.That(token.Type, Is.EqualTo(XmlTokenType.Declaration));
        }

        [Test]
        public void GetTokens_Correctly_Parses_Start_Tags()
        {
            string element = "<stream:stream xmlns='jabber:client' xmlns:stream='http://etherx.jabber.org/streams' id='3744742480' from='redcat' version='1.0' xml:lang='en'>";
            var token = lexer.GetTokens(element).Single();

            Assert.That(token.Type, Is.EqualTo(XmlTokenType.StartTag));
            Assert.That(XmlLexer.GetTagName(token), Is.EqualTo("stream:stream"));
        }

        private string[] enclosedTags = { "<element />", "<ns:element />", "<c xmlns='http://jabber.org/protocol/caps' hash='sha-1' />" };
        
        [Test]
        public void GetTokens_Correctly_Parses_Enclosed_Tag([ValueSource("enclosedTags")]string tag)
        {
            var tokens = lexer.GetTokens(tag).ToArray();
            Assert.That(tokens.Length, Is.EqualTo(1));
            Assert.That(tokens[0].Type, Is.EqualTo(XmlTokenType.EnclosedTag));
        }

        private string[] closingTags = { "</ elem>", "</stream:features>", "</some-elem>" };

        [Test]
        public void GetTokens_Correctly_Parses_Closing_Tags([ValueSource("closingTags")]string tag)
        {
            var token = lexer.GetTokens(tag).Single();
            Assert.That(token.Type, Is.EqualTo(XmlTokenType.ClosingTag));
        }

        private string[] elementsWithValue = { "<node>Value</node>", "<node attr1='val0' attr2='val'>Value</node>" };

        [Test]
        public void GetTokens_Correctly_Parses_Element_With_Value([ValueSource("elementsWithValue")]string element)
        {
            var tokens = lexer.GetTokens(element).ToArray();

            Assert.That(tokens.Length, Is.EqualTo(3));
            Assert.That(tokens[0].Type, Is.EqualTo(XmlTokenType.StartTag));
            Assert.That(tokens[1].Type, Is.EqualTo(XmlTokenType.Value));
            Assert.That(tokens[1].Value, Is.EqualTo("Value"));
            Assert.That(tokens[2].Type, Is.EqualTo(XmlTokenType.ClosingTag));
        }

        [Test]
        public void GetTokens_Correctly_Parses_Element_With_Childs()
        {
            string element = @"<root ><child1 /><child2>Fer kol</ child0><child3 atr=""1"" art='rtr'/></root>";

            var tokens = lexer.GetTokens(element).ToArray();

            Assert.That(tokens.Length, Is.EqualTo(7));
            AssertToken(tokens[0], XmlTokenType.StartTag, "root");
            AssertToken(tokens[1], XmlTokenType.EnclosedTag, "child1");
            AssertToken(tokens[2], XmlTokenType.StartTag, "child2");
            AssertToken(tokens[3], XmlTokenType.Value, "Fer kol");
            AssertToken(tokens[4], XmlTokenType.ClosingTag, "child0");
            AssertToken(tokens[5], XmlTokenType.EnclosedTag, "child3");
            AssertToken(tokens[6], XmlTokenType.ClosingTag, "root");
        }

        private void AssertToken(XmlToken token, XmlTokenType type, string nameOrValue)
        {
            Assert.That(token.Type, Is.EqualTo(type));
            if (token.Type != XmlTokenType.Value) Assert.That(XmlLexer.GetTagName(token), Is.EqualTo(nameOrValue));
            else Assert.That(token.Value, Is.EqualTo(nameOrValue));
        }

        [Test, Ignore]
        public void GetTokens_Correctly_Parses_Whitespaces()
        {
            string xml = "\t<element/>\n <a>Val</a> ";

            var tokens = lexer.GetTokens(xml).ToArray();

            Assert.That(tokens[0].Type, Is.EqualTo(XmlTokenType.Whitespace));
            Assert.That(tokens[2].Type, Is.EqualTo(XmlTokenType.Whitespace));
            Assert.That(tokens[6].Type, Is.EqualTo(XmlTokenType.Whitespace));
        }

        [Test]
        public void GetTokens_Parses_Tag_Names_If_Option_Turned_On()
        {
            lexer.Options.ParseTagName = true;
            var token = lexer.GetTokens("<some-element>").Single();

            Assert.That(token.TagName, Is.EqualTo("some-element"));
        }

        [Test]
        public void Does_Not_Parses_TagName_For_Non_Tag_Tokens()
        {
            lexer.Options.ParseTagName = true;
            var value = lexer.GetTokens("<e>value</e>")[1];

            Assert.That(value.TagName, Is.Null);
        }

        private Tuple<string, string>[] dataForGetTagName =
        {
            new Tuple<string, string>(@"<kitty />", "kitty"), 
            new Tuple<string, string>(@"<k:itty>", "k:itty"),
            new Tuple<string, string>(@"<kitt-y8 attr1='val1' attr0='val0'>", "kitt-y8") 
        };

        #endregion

        [Test]
        public void GetTagName_Returns_Tag_Name([ValueSource("dataForGetTagName")]Tuple<string, string> data)
        {
            XmlToken token = lexer.GetTokens(data.Item1).Single();

            string name = XmlLexer.GetTagName(token);

            Assert.That(name, Is.EqualTo(data.Item2));
        }

        [Test]
        public void GetTagAttributes_Returns_Empty_Collection_For_Tag_Without_Attributes()
        {
            XmlToken token = lexer.GetTokens("<someone >").Single();

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
            var token = lexer.GetTokens(tag).Single();

            var actualAttributes = XmlLexer.GetTagAttributes(token).ToArray();

            Assert.That(actualAttributes, Is.EquivalentTo(expectedAttributes));
        }
    }
}
