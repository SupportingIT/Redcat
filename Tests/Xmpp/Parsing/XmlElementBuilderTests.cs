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
            Assert.That(element.Value, Is.Null);
        }

        [Test]
        public void Can_Build_Simple_Element_With_Attributes()
        {
            XmlElementBuilder builder = new XmlElementBuilder();

            builder.NewElement("some-elem");
            builder.AddAttribute("attr1", "val1");
            builder.AddAttribute("attr2", "val2");
            var element = builder.Element;
            var attributes = element.Attributes.ToArray();

            Assert.That(attributes.Length, Is.EqualTo(2));
            Assert.That(attributes[0].Name, Is.EqualTo("attr1"));
            Assert.That(attributes[0].Value, Is.EqualTo("val1"));
            Assert.That(attributes[1].Name, Is.EqualTo("attr2"));
            Assert.That(attributes[1].Value, Is.EqualTo("val2"));
        }

        [Test]
        public void Can_Build_Element_With_Value()
        {
            XmlElementBuilder builder = new XmlElementBuilder();

            builder.NewElement("elem");
            builder.SetNodeValue("some-value");
            var element = builder.Element;

            Assert.That(element.Value, Is.EqualTo("some-value"));
        }

        [Test]
        public void Can_Build_Element_With_Childs()
        {
            XmlElementBuilder builder = new XmlElementBuilder();

            builder.NewElement("root-element");
            builder.StartNode("node0");
            builder.EndNode();
            builder.StartNode("node1");
            builder.EndNode();
            builder.StartNode("node2");
            builder.EndNode();

            var childs = builder.Element.Childs.ToArray();

            Assert.That(childs[0].Name, Is.EqualTo("node0"));
            Assert.That(childs[1].Name, Is.EqualTo("node1"));
            Assert.That(childs[2].Name, Is.EqualTo("node2"));
        }

        [Test]
        public void Can_Build_Child_Elements_With_Attributes()
        {
            XmlElementBuilder builder = new XmlElementBuilder();

            builder.NewElement("elem");
            builder.StartNode("node-1");
            builder.AddAttribute("attr1", "val1");
            builder.AddAttribute("attr2", "val2");

            var child = builder.Element.Childs.Single();

            Assert.That(child.GetAttributeValue<string>("attr1"), Is.EqualTo("val1"));
            Assert.That(child.GetAttributeValue<string>("attr2"), Is.EqualTo("val2"));
        }

        [Test]
        public void Can_Build_Child_Elements_With_Values()
        {
            XmlElementBuilder builder = new XmlElementBuilder();

            builder.NewElement("elem");
            builder.StartNode("node1");
            builder.SetNodeValue("value1");
            builder.EndNode();

            var child = builder.Element.Childs.Single();

            Assert.That(child.Attributes.Count(), Is.EqualTo(0));
            Assert.That(child.Value, Is.EqualTo("value1"));
        }

        [Test]
        public void Can_Build_Child_Elements_With_childs()
        {
            XmlElementBuilder builder = new XmlElementBuilder();

            builder.NewElement("elem");
            builder.StartNode("node1");
            builder.StartNode("node1-1");

            var child = builder.Element.Childs.Single().Childs.Single();

            Assert.That(child.Name, Is.EqualTo("node1-1"));
        }
    }
}
