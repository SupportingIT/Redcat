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
        public void Uses_Correct_Parser_For_Xml_Element()
        {
            IElementParser parser1 = A.Fake<IElementParser>();
            IElementParser parser2 = A.Fake<IElementParser>();

            A.CallTo(() => parser1.CanParse("element2")).Returns(false);
            A.CallTo(() => parser2.CanParse("element2")).Returns(true);

            CreateAndRunReader("<element2></element2>", parser1, parser2);
                        
            A.CallTo(() => parser2.CanParse("element2")).MustHaveHappened();
            A.CallTo(() => parser2.NewElement("element2")).MustHaveHappened();
        }

        [Test]
        public void Correctly_Parses_Xml_Attributes()
        {
            IElementParser parser = A.Fake<IElementParser>();
            A.CallTo(() => parser.CanParse("element")).Returns(true);

            CreateAndRunReader("<element attribute1='value1' attribute2='value2' />", parser);

            A.CallTo(() => parser.AddAttribute("attribute1", "value1")).MustHaveHappened();
            A.CallTo(() => parser.AddAttribute("attribute2", "value2")).MustHaveHappened();
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Throws_Exception_If_No_Parsers_For_Element()
        {
            IElementParser parser = A.Fake<IElementParser>();
            A.CallTo(() => parser.CanParse("element1")).Returns(true);

            CreateAndRunReader("<element2></element2>", parser);
        }

        [Test]
        public void Set_Correct_Node_Value()
        {
            IElementParser parser = A.Fake<IElementParser>();
            A.CallTo(() => parser.CanParse("element")).Returns(true);

            CreateAndRunReader("<element>value0</element>", parser);

            A.CallTo(() => parser.SetNodeValue("value0")).MustHaveHappened();
        }

        [Test]
        public void Correctly_Parses_Attributes_For_Inner_Elements()
        {
            IElementParser parser = A.Fake<IElementParser>();
            A.CallTo(() => parser.CanParse(A<string>.Ignored)).Returns(true);

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
            IElementParser parser = A.Fake<IElementParser>();
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
        public void Returns_Parsed_Element()
        {
            IElementParser parser = A.Fake<IElementParser>();
            Element element = A.Fake<Element>();
            XmppStreamReader reader = CreateReader("<element01>val</element01>", parser);

            A.CallTo(() => parser.ParsedElement).Returns(element);
            A.CallTo(() => parser.CanParse(A<string>.Ignored)).Returns(true);

            Element parsedElement = reader.Read();
            
            Assert.That(element, Is.SameAs(parsedElement));
        }

        private XmppStreamReader CreateAndRunReader(string xml, params IElementParser[] parsers)
        {
            XmppStreamReader reader = CreateReader(xml, parsers);
            reader.Read();
            return reader;
        }

        private XmppStreamReader CreateReader(string xml, params IElementParser[] parsers)
        {
            Stream stream = CreateStream(xml);
            XmppStreamReader reader = new XmppStreamReader(stream);
            foreach (var parser in parsers) reader.Parsers.Add(parser);
            return reader;
        }

        private Stream CreateStream(string content)
        {
            MemoryStream stream = new MemoryStream();
            byte[] bytes = Encoding.Unicode.GetBytes(content);
            stream.Write(bytes, 0, bytes.Length);
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }
    }
}
