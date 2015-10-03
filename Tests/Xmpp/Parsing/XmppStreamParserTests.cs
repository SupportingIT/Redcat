using NUnit.Framework;
using Redcat.Xmpp.Parsing;
using System.Linq;

namespace Redcat.Xmpp.Tests.Parsing
{
    [TestFixture]
    public class XmppStreamParserTests
    {
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

            var element = parser.Parse("<el>val</el>").ElementAt(1);

            Assert.That(element.Name, Is.EqualTo("el"));
            Assert.That(element.Childs, Is.Empty);
            Assert.That(element.Value, Is.EqualTo("val"));
        }
    }
}
