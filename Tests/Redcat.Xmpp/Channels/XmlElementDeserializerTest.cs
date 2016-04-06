using FakeItEasy;
using NUnit.Framework;
using Redcat.Xmpp.Channels;
using Redcat.Xmpp.Parsing;
using System;
using System.Text;

namespace Redcat.Xmpp.Tests.Channels
{
    [TestFixture]
    public class XmlElementDeserializerTest
    {
        [Test]
        public void Read_Parses_Completed_Elements()
        {
            IXmlParser parser = A.Fake<IXmlParser>();
            XmlElementDeserializer deserializer = new XmlElementDeserializer(parser, 1024);
            string xmlFragment = "<element1 /><elem2>";
            byte[] buffer = Encoding.UTF8.GetBytes(xmlFragment);

            deserializer.Read(new ArraySegment<byte>(buffer));

            Assert.Fail();
        }
    }
}
