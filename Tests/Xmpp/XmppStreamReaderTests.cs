using NUnit.Framework;
using FakeItEasy;
using System.IO;
using System.Text;
using System;
using Redcat.Xmpp.Parsing;
using Redcat.Xmpp.Xml;

namespace Redcat.Xmpp.Tests
{
    [TestFixture]
    public class XmppStreamReaderTests
    {
        [Test]
        public void Uses_Correct_Builder_For_Xml_Element()
        {
            IXmlElementBuilder parser1 = A.Fake<IXmlElementBuilder>();
            IXmlElementBuilder parser2 = A.Fake<IXmlElementBuilder>();

            A.CallTo(() => parser1.CanParse("element2")).Returns(false);
            A.CallTo(() => parser2.CanParse("element2")).Returns(true);

            CreateAndRunReader("<element2></element2>", parser1, parser2);
                        
            A.CallTo(() => parser2.CanParse("element2")).MustHaveHappened();
            A.CallTo(() => parser2.NewElement("element2")).MustHaveHappened();
        }

        [Test]
        public void Correctly_Builds_Xml_Attributes()
        {
            IXmlElementBuilder parser = A.Fake<IXmlElementBuilder>();
            A.CallTo(() => parser.CanParse("element")).Returns(true);

            CreateAndRunReader("<element attribute1='value1' attribute2='value2' />", parser);

            A.CallTo(() => parser.AddAttribute("attribute1", "value1")).MustHaveHappened();
            A.CallTo(() => parser.AddAttribute("attribute2", "value2")).MustHaveHappened();
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Throws_Exception_If_No_Builders_For_Element()
        {
            IXmlElementBuilder parser = A.Fake<IXmlElementBuilder>();
            A.CallTo(() => parser.CanParse("element1")).Returns(true);

            CreateAndRunReader("<element2></element2>", parser);
        }

        [Test]
        public void Set_Correct_Node_Value()
        {
            IXmlElementBuilder parser = A.Fake<IXmlElementBuilder>();
            A.CallTo(() => parser.CanParse("element")).Returns(true);

            CreateAndRunReader("<element>value0</element>", parser);

            A.CallTo(() => parser.SetNodeValue("value0")).MustHaveHappened();
        }

        [Test]
        public void Correctly_Builds_Attributes_For_Inner_Elements()
        {
            IXmlElementBuilder parser = A.Fake<IXmlElementBuilder>();
            A.CallTo(() => parser.CanParse(A<string>._)).Returns(true);

            CreateAndRunReader(@"<root>
                                    <child1 attr1='value1' />
                                    <child2 attr2='value2' />
                                 </root>", parser);
            
            A.CallTo(() => parser.NewElement("root")).MustHaveHappened();
            A.CallTo(() => parser.StartNode("child1")).MustHaveHappened();
            A.CallTo(() => parser.AddAttribute("attr1", "value1")).MustHaveHappened();
            A.CallTo(() => parser.EndNode()).MustHaveHappened();
            A.CallTo(() => parser.StartNode("child2")).MustHaveHappened();
            A.CallTo(() => parser.AddAttribute("attr2", "value2")).MustHaveHappened();
            A.CallTo(() => parser.EndNode()).MustHaveHappened();            
        }

        [Test]
        public void Correctly_Set_Values_And_Attributes_For_Inner_Elements()
        {
            IXmlElementBuilder parser = A.Fake<IXmlElementBuilder>();
            A.CallTo(() => parser.CanParse(A<string>.Ignored)).Returns(true);

            CreateAndRunReader(@"<root>
                                    <child1 attr0='val0'>value0</child1>
                                    <child2 attr1='val1'>value1</child2>
                                 </root>", parser);

            A.CallTo(() => parser.StartNode("child1")).MustHaveHappened();
            A.CallTo(() => parser.AddAttribute("attr0", "val0")).MustHaveHappened();
            A.CallTo(() => parser.SetNodeValue("value0")).MustHaveHappened();
            A.CallTo(() => parser.EndNode()).MustHaveHappened();
            A.CallTo(() => parser.StartNode("child2")).MustHaveHappened();
            A.CallTo(() => parser.AddAttribute("attr1", "val1")).MustHaveHappened();
            A.CallTo(() => parser.SetNodeValue("value1")).MustHaveHappened();
            A.CallTo(() => parser.EndNode()).MustHaveHappened();
        }

        [Test]
        public void Returns_Builded_Element()
        {
            IXmlElementBuilder parser = A.Fake<IXmlElementBuilder>();
            XmlElement element = A.Fake<XmlElement>();
            XmppStreamReader reader = CreateReader("<element01>val</element01>", parser);

            A.CallTo(() => parser.Element).Returns(element);
            A.CallTo(() => parser.CanParse(A<string>.Ignored)).Returns(true);

            XmlElement parsedElement = reader.Read();
            
            Assert.That(element, Is.SameAs(parsedElement));
        }

        private XmppStreamReader CreateAndRunReader(string xml, params IXmlElementBuilder[] parsers)
        {
            XmppStreamReader reader = CreateReader(xml, parsers);
            reader.Read();
            return reader;
        }

        private XmppStreamReader CreateReader(string xml, params IXmlElementBuilder[] parsers)
        {
            Stream stream = CreateStream(xml);
            XmppStreamReader reader = new XmppStreamReader(new StreamReader(stream, Encoding.UTF8));
            foreach (var parser in parsers) reader.Builders.Add(parser);
            return reader;
        }

        private Stream CreateStream(string content)
        {
            MemoryStream stream = new MemoryStream();
            byte[] bytes = Encoding.UTF8.GetBytes(content);
            stream.Write(bytes, 0, bytes.Length);
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }
    }
}
