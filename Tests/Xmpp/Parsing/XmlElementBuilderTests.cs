using System;
using System.Linq;
using NUnit.Framework;
using Redcat.Xmpp.Parsing;

namespace Redcat.Xmpp.Tests.Parsing
{
    [TestFixture]
    public class XmlElementBuilderTests
    {
        private string[] simpleElementNames = { "element", "some-element", "prefix:name" };

        [Test]
        public void Can_Build_Simple_Element([ValueSource("simpleElementNames")]string elementName)
        {
            XmlElementBuilder builder = new XmlElementBuilder();

            builder.NewElement(elementName);
            var element = builder.Element;

            Assert.That(element.Name, Is.EqualTo(elementName));
            Assert.That(element.Attributes.Count(), Is.EqualTo(0));
        }
    }
}
