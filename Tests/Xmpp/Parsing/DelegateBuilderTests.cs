using System;
using NUnit.Framework;
using Redcat.Xmpp.Parsing;
using Redcat.Xmpp.Xml;

namespace Redcat.Xmpp.Tests.Parsing
{
    [TestFixture]
    public class DelegateBuilderTests
    {
        [Test]
        public void Attribute_Adds_Attribute_Setter()
        {
            DelegateBuilderImpl builder = new DelegateBuilderImpl("elem");
            builder.AddAttributeSetter("attr1", (e, v) => e.SetAttributeValue("attr1", int.Parse(v)));

            builder.NewElement("elem");
            builder.AddAttribute("attr1", "1");
            var element = builder.Element;

            Assert.That(element.GetAttributeValue<int>("attr1"), Is.EqualTo(1));
        }

        [Test]
        public void AddAttribute_Sets_Attribute_Value_If_No_Setters()
        {
            DelegateBuilderImpl builder = new DelegateBuilderImpl("elem");
            builder.AddAttributeSetter("attr0", (e, v) => e.SetAttributeValue("attr0", 8));

            builder.NewElement("elem");
            builder.AddAttribute("attr1", "value1");
            var element = builder.Element;

            Assert.That(element.GetAttributeValue<int>("attr0"), Is.EqualTo(0));
            Assert.That(element.GetAttributeValue<string>("attr1"), Is.EqualTo("value1"));
        }

        [Test]
        public void NodeValue_Adds_Node_Value_Setter()
        {
            DelegateBuilderImpl builder = new DelegateBuilderImpl("elem");
            string actualValue = null;
            builder.AddNodeValueSetter("node1", (e, v) => actualValue = v);

            builder.NewElement("elem");
            builder.StartNode("node1");
            builder.SetNodeValue("value1");
            var element = builder.Element;

            Assert.That(actualValue, Is.EqualTo("value1"));
        }

        [Test]
        public void SetNodeValue_Does_Not_Throws_Exception()
        {
            DelegateBuilderImpl builder = new DelegateBuilderImpl("elem");

            builder.NewElement("elem");
            builder.StartNode("node0");
            builder.SetNodeValue("value0");
        }
    }

    internal class DelegateBuilderImpl : DelegateBuilder<XmlElement>
    {
        public DelegateBuilderImpl(params string[] elems) : base(elems)
        { }

        public void AddAttributeSetter(string name, Action<XmlElement, string> setter)
        {
            Attribute(name, setter);
        }

        public void AddNodeValueSetter(string name, Action<XmlElement, string> setter)
        {
            NodeValue(name, setter);
        }

        protected override XmlElement CreateInstance(string elementName)
        {
            return new XmlElement(elementName);
        }
    }
}
