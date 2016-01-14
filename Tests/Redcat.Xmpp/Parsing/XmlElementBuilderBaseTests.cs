using System;
using NUnit.Framework;
using Redcat.Xmpp.Parsing;
using Redcat.Xmpp.Xml;

namespace Redcat.Xmpp.Tests.Parsing
{
    [TestFixture]
    public class XmlElementBuilderBaseTests
    {
        [Test]
        public void CanParse_Returns_Correct_Result()
        {
            var parser = new XmlElementBuilderBaseImpl("elem", "e0");

            Assert.That(parser.CanBuild("elem"), Is.True);
            Assert.That(parser.CanBuild("element2"), Is.False);
            Assert.That(parser.CanBuild("e0"), Is.True);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void NewElement_Throws_Exception_If_Unsupported_Name_Specified()
        {
            var builder = new XmlElementBuilderBaseImpl("elem0");

            builder.NewElement("elem1");
        }

        [Test]
        public void NewElement_Calls_CreateElement_With_Correct_Parameter()
        {
            string expectedName = "some-good-name";
            XmlElementBuilderBaseImpl builder = new XmlElementBuilderBaseImpl(expectedName);
            string actualName = null;
            builder.SetCreateElementFunc(n =>
            {
                actualName = n;
                return null;
            });

            builder.NewElement(expectedName);

            Assert.That(actualName, Is.EqualTo(expectedName));
        }

        [Test]
        public void NewElement_Sets_Element_Property_After_Calling_NewElement()
        {
            var builder = new XmlElementBuilderBaseImpl("elem");
            Assert.That(builder.Element, Is.Null);
            XmlElement element = new XmlElement("elem");
            builder.SetCreateElementFunc(n => element);

            builder.NewElement("elem");

            Assert.That(builder.Element, Is.EqualTo(element));
        }

        [Test]
        public void AddAttribute_Sets_AttributeName_And_Attribute_Value()
        {
            var builder = new XmlElementBuilderBaseImpl("elem");
            string nodeName = null;
            string attributeName = null;
            string attributeValue = null;
            builder.SetAddAttribute(c =>
            {
                nodeName = c.NodeName;
                attributeName = c.AttributeName;
                attributeValue = c.AttributeValue;
            });

            builder.NewElement("elem");
            builder.StartNode("node1");
            builder.AddAttribute("attr0", "val01");

            Assert.That(nodeName, Is.EqualTo("node1"));
            Assert.That(attributeName, Is.EqualTo("attr0"));
            Assert.That(attributeValue, Is.EqualTo("val01"));
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void StartNode_Throws_Exception_If_Name_Is_Null_Or_Empty([Values(null, "")] string name)
        {
            var builder = new XmlElementBuilderBaseImpl("elem");
            builder.NewElement("elem");
            builder.StartNode(name);
        }

        [Test]
        public void StartNode_Increases_Depth_And_Correctly_Sets_NodeName_For_Context()
        {
            var builder = new XmlElementBuilderBaseImpl("elem");
            BuilderContext context = null;
            builder.SetOnStartNode(c => context = c);
            
            builder.NewElement("elem");
            builder.StartNode("node0");
            Assert.That(context.NodeName, Is.EqualTo("node0"));
            Assert.That(context.Depth, Is.EqualTo(1));
            builder.StartNode("good-node");
            Assert.That(context.NodeName, Is.EqualTo("good-node"));
            Assert.That(context.Depth, Is.EqualTo(2));
        }

        [Test]
        public void SetNodeValue_Correctly_Sets_NodeName_And_NodeValue_For_Context()
        {
            var builder = new XmlElementBuilderBaseImpl("elem");
            string nodeName = null;
            string nodeValue = null;
            builder.SetOnSetNodeValue(c =>
            {
                nodeName = c.NodeName;
                nodeValue = c.NodeValue;
            });

            builder.NewElement("elem");
            builder.StartNode("node8");
            builder.SetNodeValue("some-value8");

            Assert.That(nodeName, Is.EqualTo("node8"));
            Assert.That(nodeValue, Is.EqualTo("some-value8"));
        }

        [Test]
        public void EndNode_Decreases_Depth_And_Correctly_Sets_NodeName_For_Context()
        {
            var builder = new XmlElementBuilderBaseImpl("element");
            int depth = -1;
            string nodeName = null;
            builder.SetOnEndNode(c => { depth = c.Depth; nodeName = c.NodeName; });
            builder.NewElement("element");
            builder.StartNode("node1");
            builder.StartNode("node11");
            builder.StartNode("node8");

            builder.EndNode();
            Assert.That(nodeName, Is.EqualTo("node8"));
            Assert.That(depth, Is.EqualTo(3));

            builder.EndNode();
            Assert.That(nodeName, Is.EqualTo("node11"));
            Assert.That(depth, Is.EqualTo(2));

            builder.EndNode();
            Assert.That(nodeName, Is.EqualTo("node1"));
            Assert.That(depth, Is.EqualTo(1));

            builder.EndNode();
            Assert.That(nodeName, Is.EqualTo("element"));
            Assert.That(depth, Is.EqualTo(0));
        }

        internal class XmlElementBuilderBaseImpl : XmlElementBuilderBase
        {
            private Func<string, XmlElement> createFunc;
            private Action<BuilderContext> addAttribute; 
            private Action<BuilderContext> onStartNode;
            private Action<BuilderContext> onSetNodeValue; 
            private Action<BuilderContext> onEndNode;

            public XmlElementBuilderBaseImpl(params string[] supportedElements) : base(supportedElements)
            {
                createFunc = n => new XmlElement(n);
                onStartNode = c => { };
            }

            protected override XmlElement CreateElement(string elementName)
            {
                return createFunc(elementName);
            }

            protected override void OnAddAttribute(BuilderContext context)
            {
                addAttribute(context);
            }

            protected override void OnStartNode(BuilderContext context)
            {
                onStartNode(context);
            }

            protected override void OnSetNodeValue(BuilderContext context)
            {
                onSetNodeValue(context);
            }

            protected override void OnEndNode(BuilderContext context)
            {
                onEndNode(context);
            }

            public void SetOnSetNodeValue(Action<BuilderContext> action)
            {
                onSetNodeValue = action;
            }

            public void SetAddAttribute(Action<BuilderContext> action)
            {
                addAttribute = action;
            }

            public void SetOnEndNode(Action<BuilderContext> action)
            {
                onEndNode = action;
            }

            public void SetOnStartNode(Action<BuilderContext> action)
            {
                onStartNode = action;
            }

            public void SetCreateElementFunc(Func<string, XmlElement> createFunc)
            {
                this.createFunc = createFunc;
            }
        }
    }
}
