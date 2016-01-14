using System.Collections.Generic;
using System.IO;
using System.Linq;
using FakeItEasy;
using NUnit.Framework;
using Redcat.Xmpp.Parsing;
using Redcat.Xmpp.Xml;
using System;

namespace Redcat.Xmpp.Tests
{
    [TestFixture]
    public class XmppStreamReaderTests
    {
        [Test]
        public void Read_Returns_Elements_In_The_Same_Order_As_Returned_By_Parser()
        {
            ValidateReader(r => r.Read());
        }

        [Test]
        public void ReadAsync_Returns_Elements_In_The_Same_Order_As_Returned_By_Parser()
        {
            ValidateReader(r => r.ReadAsync().Result);
        }

        private void ValidateReader(Func<XmppStreamReader, XmlElement> read)
        {
            var expectedElements = Enumerable.Range(0, 3).Select(i => new XmlElement("name" + i)).ToArray();
            var parser = A.Fake<IXmlParser>();
            A.CallTo(() => parser.Parse(A<string>._)).Returns(expectedElements).Once();
            TextReader textReader = new StringReader("HelloWorld");
            XmppStreamReader reader = new XmppStreamReader(textReader) { Parser = parser };
            List<XmlElement> actualElements = new List<XmlElement>();

            for (int i = 0; i < expectedElements.Length; i++) actualElements.Add(read(reader));

            Assert.That(actualElements, Is.EqualTo(expectedElements));
            Assert.That(reader.Read(), Is.Null);
        }
    }
}
