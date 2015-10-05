using NUnit.Framework;
using Redcat.Xmpp.Parsing;
using Redcat.Xmpp.Xml;
using System;
using System.Linq;

namespace Redcat.Xmpp.Tests.Parsing
{
    [TestFixture]
    public class XmppStreamParserTests
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Parse_Throws_Exception_If_Null_Argument()
        {
            XmppStreamParser parser = new XmppStreamParser();
            parser.Parse(null);
        }

        [Test]
        public void Parse_Returns_Empty_Collection_If_Given_String_Is_Empty()
        {
            XmppStreamParser parser = new XmppStreamParser();

            var elements = parser.Parse(string.Empty);

            Assert.That(elements, Is.Empty);
        }

        [Test]
        public void Can_Parse_Single_Element_Without_Attributes_And_Value()
        {
            XmppStreamParser parser = new XmppStreamParser();

            var element = parser.Parse("<elem/>").Single();

            Assert.That(element.Name, Is.EqualTo("elem"));
            Assert.That(element.Attributes, Is.Empty);
            Assert.That(element.Value, Is.Null);
        }

        [Test]
        public void Can_Parse_Single_Element_With_Attributes()
        {
            XmppStreamParser parser = new XmppStreamParser();

            var element = parser.Parse("<e a1='v1' a2='v2'/>").Single();

            Assert.That(element.Name, Is.EqualTo("e"));
            Assert.That(element.GetAttributeValue<string>("a1"), Is.EqualTo("v1"));
            Assert.That(element.GetAttributeValue<string>("a2"), Is.EqualTo("v2"));
        }

        [Test]
        public void Can_Parse_Single_Element_With_Value()
        {
            XmppStreamParser parser = new XmppStreamParser();

            var element = parser.Parse("<el>val</el>").Single();

            Assert.That(element.Name, Is.EqualTo("el"));
            Assert.That(element.Childs, Is.Empty);
            Assert.That(element.Value, Is.EqualTo("val"));
        }

        [Test]
        public void Can_Parse_Single_Element_With_Childs()
        {
            XmppStreamParser parser = new XmppStreamParser();

            var element = parser.Parse("<e0 a0='0' a2='-1'><e1 /><e2 attr1='a' a2='b'/><e3>val</e3></e0>").Single();

            Assert.That(element.Attributes.Count(), Is.EqualTo(2));
            Assert.That(element.GetAttributeValue<string>("a0"), Is.EqualTo("0"));
            Assert.That(element.GetAttributeValue<string>("a2"), Is.EqualTo("-1"));

            var childs = element.Childs.ToArray();

            Assert.That(childs.Length, Is.EqualTo(3));
            AssertOnlyName(childs[0], "e1");
            AssertOnlyNameAndValue(childs[2], "e3", "val");

            Assert.That(childs[1].Name, Is.EqualTo("e2"));
            Assert.That(childs[1].Attributes.Count(), Is.EqualTo(2));
            Assert.That(childs[1].GetAttributeValue<string>("attr1"), Is.EqualTo("a"));
            Assert.That(childs[1].GetAttributeValue<string>("a2"), Is.EqualTo("b"));
            Assert.That(childs[1].Value, Is.Null);
            Assert.That(childs[1].Childs, Is.Empty);
        }

        [Test]
        public void Can_Parse_Many_Elements()
        {
            XmppStreamParser parser = new XmppStreamParser();

            var elements = parser.Parse("<el1/><elem2>value2</elem2><element3 atr0='v1' attr='g2'/>")
                                 .ToArray();

            Assert.That(elements.Length, Is.EqualTo(3));

            AssertOnlyName(elements[0], "el1");
            AssertOnlyNameAndValue(elements[1], "elem2", "value2");

            Assert.That(elements[2].Name, Is.EqualTo("element3"));
            Assert.That(elements[2].Attributes.Count(), Is.EqualTo(2));
            Assert.That(elements[2].GetAttributeValue<string>("atr0"), Is.EqualTo("v1"));
            Assert.That(elements[2].GetAttributeValue<string>("attr"), Is.EqualTo("g2"));
        }

        [Test]
        public void Can_Parse_Stream_Header()
        {
            XmppStreamParser parser = new XmppStreamParser();

            var elements = parser.Parse(@"<stream:stream xmlns='jabber:client' xmlns:stream='http://etherx.jabber.org/streams'><stream:features><feature1/><feature2/></stream:features>").ToArray();
            
            Assert.That(elements.Length, Is.EqualTo(2));
            Assert.That(elements[0].Name, Is.EqualTo("stream:stream"));
            Assert.That(elements[1].Name, Is.EqualTo("stream:features"));
        }

        private void AssertOnlyName(XmlElement element, string name)
        {
            Assert.That(element.Name, Is.EqualTo(name));
            Assert.That(element.Attributes, Is.Empty);
            Assert.That(element.Value, Is.Null);
            Assert.That(element.Childs, Is.Empty);
        }

        private void AssertOnlyNameAndValue(XmlElement element, string name, string value)
        {
            Assert.That(element.Name, Is.EqualTo(name));
            Assert.That(element.Attributes, Is.Empty);
            Assert.That(element.Value, Is.EqualTo(value));
            Assert.That(element.Childs, Is.Empty);
        }
    }
}
