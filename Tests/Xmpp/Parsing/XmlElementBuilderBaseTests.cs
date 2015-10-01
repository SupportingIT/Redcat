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
            XmlElementBuilderBase parser = new XmlElementBuilderBaseImpl("elem", "e0");

            Assert.That(parser.CanBuild("elem"), Is.True);
            Assert.That(parser.CanBuild("element2"), Is.False);
            Assert.That(parser.CanBuild("e0"), Is.True);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void NewElement_Throws_Exception_If_Unsupported_Name_Specified()
        {
            XmlElementBuilderBase builder = new XmlElementBuilderBaseImpl("elem0");

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
            XmlElementBuilderBaseImpl builder = new XmlElementBuilderBaseImpl("elem");
            Assert.That(builder.Element, Is.Null);
            XmlElement element = new XmlElement("elem");
            builder.SetCreateElementFunc(n => element);

            builder.NewElement("elem");

            Assert.That(builder.Element, Is.EqualTo(element));
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
            XmlElementBuilderBaseImpl builder = new XmlElementBuilderBaseImpl("elem");
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

        internal class XmlElementBuilderBaseImpl : XmlElementBuilderBase
        {
            private Func<string, XmlElement> createFunc;
            private Action<BuilderContext> onStartNode;

            public XmlElementBuilderBaseImpl(params string[] supportedElements) : base(supportedElements)
            {
                createFunc = n => new XmlElement(n);
            }

            protected override XmlElement CreateElement(string elementName)
            {
                return createFunc(elementName);
            }

            protected override void OnStartNode(BuilderContext context)
            {
                onStartNode(context);
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
