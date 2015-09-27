using System;
using FakeItEasy;
using NUnit.Framework;
using Redcat.Xmpp.Parsing;
using Redcat.Xmpp.Xml;

namespace Redcat.Xmpp.Tests.Parsing
{
    [TestFixture]
    public class DelegateParserTests
    {
        [Test]
        public void NewElement_Correctly_Uses_CreateFunc()
        {
            XmlElement xmlElement = A.Fake<XmlElement>();
            string expectedElementName = "elem";
            string actualElementName = null;
            DelegateParser<XmlElement> parser = new DelegateParser<XmlElement>(e => 
            {
                actualElementName = e;
                return xmlElement;
            });

            parser.NewElement(expectedElementName);

            Assert.That(parser.ParsedElement, Is.SameAs(xmlElement));
            Assert.That(actualElementName, Is.EqualTo(expectedElementName));
        }

        [Test]
        public void CanParse_Returns_Correct_Result()
        {
            DelegateParser<XmlElement> parser = new DelegateParser<XmlElement>(s => A.Fake<XmlElement>(), "elem", "e0");

            Assert.That(parser.CanParse("elem"), Is.True);
            Assert.That(parser.CanParse("element2"), Is.False);
            Assert.That(parser.CanParse("e0"), Is.True);
        }

        [Test]
        public void AddAttribute_Correctly_Calls_Attribute_Builder()
        {
            XmlElement actualElement = null, expectedElement = A.Fake<XmlElement>();
            DelegateParser<XmlElement> parser = new DelegateParser<XmlElement>(s => expectedElement);
            string actualAttrName = null, expectedAttrName = "Attr8";
            string actualAttrValue = null, expectedAttrValue = "value08";
            
            parser.AttributeBuilders[expectedAttrName] = context => 
            {
                actualElement = context.Element;
                actualAttrName = context.AttributeName;
                actualAttrValue = context.AttributeValue;
            };

            parser.NewElement("");
            parser.AddAttribute(expectedAttrName, expectedAttrValue);

            Assert.That(expectedElement, Is.SameAs(actualElement));
            Assert.That(actualAttrName, Is.EqualTo(expectedAttrName));
            Assert.That(actualAttrValue, Is.EqualTo(expectedAttrValue));
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AddAttribute_Throws_Exception_If_NoElements_Created()
        {
            DelegateParser<XmlElement> parser = new DelegateParser<XmlElement>(s => null);
            parser.AddAttribute("name", "value");
        }

        [Test]
        public void AddAttribute_Does_Nothing_If_No_Attribute_Builders_For_Attribute_Name()
        {
            DelegateParser<XmlElement> parser = new DelegateParser<XmlElement>(s => A.Fake<XmlElement>());
            parser.NewElement("");
            parser.AddAttribute("name", "value");
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SetNodeValue_Thorow_Exception_If_StartNode_Havent_Been_called_Before()
        {
            var parser = new DelegateParser<XmlElement>(s => A.Fake<XmlElement>());
            parser.NewElement("");
            parser.SetNodeValue("value");
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SetNodeValue_Throws_Exception_If_Called_After_EndNode()
        {
            var parser = new DelegateParser<XmlElement>(s => A.Fake<XmlElement>());
            parser.NewElement("");
            parser.StartNode("node");
            parser.EndNode();
            parser.SetNodeValue("value");
        }

        [Test]
        public void SetNodeValue_Does_Nothing_If_No_NodeBuilders()
        {
            var parser = new DelegateParser<XmlElement>(s => A.Fake<XmlElement>());
            parser.NewElement("");
            parser.StartNode("some-node");
            parser.SetNodeValue("value");
        }

        [Test]
        public void SetNodeValue_Correctly_Calls_Delegate()
        {
            XmlElement actualElement = null, expectedElement = A.Fake<XmlElement>();
            string actualNodeName = null, expectedNodeName = "some-node";
            string actualNodeValue = null, expectedNodeValue = "Some-value";
            var parser = new DelegateParser<XmlElement>(s => expectedElement);
            parser.AddNodeBuilder(expectedNodeName, context =>
            {
                actualElement = context.Element;
                actualNodeName = context.NodeName;
                actualNodeValue = context.NodeValue;
            });

            parser.NewElement("XmlElement");
            parser.StartNode(expectedNodeName);
            parser.SetNodeValue(expectedNodeValue);

            Assert.That(actualElement, Is.SameAs(expectedElement));
            Assert.That(actualNodeName, Is.EqualTo(expectedNodeName));
            Assert.That(actualNodeValue, Is.EqualTo(expectedNodeValue));
        }
    }
}
