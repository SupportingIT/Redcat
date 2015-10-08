using System.Collections.Generic;
using System.Linq;
using System.Net;
using FakeItEasy;
using NUnit.Framework;
using Redcat.Core;
using Redcat.Core.Net;
using Redcat.Xmpp.Parsing;
using Redcat.Xmpp.Xml;

namespace Redcat.Xmpp.Tests
{
    [TestFixture]
    public class StreamInitializerTests
    {
        [Test]
        public void Start_Sends_Valid_Stream_Header()
        {
            var stream = new TestXmppStream();
            ConnectionSettings settings = new ConnectionSettings { Domain = "test-domain" };
            StreamInitializer initializer = new StreamInitializer(settings);

            initializer.Start(stream);

            var header = stream.SendedElements.Dequeue();

            Assert.That(header.Name, Is.EqualTo("stream:stream"));
            Assert.That(header.Xmlns, Is.EqualTo(Namespaces.JabberClient));
            Assert.That(header.GetAttributeValue<string>("xmlns:stream"), Is.EqualTo(Namespaces.Streams));
            Assert.That(header.GetAttributeValue<JID>("to"), Is.EqualTo((JID)settings.Domain));
        }

        private string[] invalidHeaders =
        {
            "<stream:stream from='test-domain' xmlns='invalid-xmlns' xmlns:stream='http://etherx.jabber.org/streams'>",
            "<stream:stream from='test-domain' xmlns='jabber:client' xmlns:stream='invalid-xmlns-stream'>",
            "<invalid:name from='test-domain' xmlns='jabber:client' xmlns:stream='http://etherx.jabber.org/streams'/>",
            "<stream:stream from='invalid-domain' xmlns='jabber:client' xmlns:stream='http://etherx.jabber.org/streams'>"
        };

        [Test]
        [ExpectedException(typeof(ProtocolViolationException))]
        public void Throws_Exception_If_Invalid_Header_Received([ValueSource("invalidHeaders")]string headerXml)
        {
            var stream = new TestXmppStream();
            ConnectionSettings settings = new ConnectionSettings { Domain = "test-domain" };
            StreamInitializer initializer = new StreamInitializer(settings);
            stream.ReceivedElements.Enqueue(Parse(headerXml));
            
            initializer.Start(stream);
        }

        private XmlElement Parse(string xml)
        {
            return new XmppStreamParser().Parse(xml).Single();
        }

        internal class TestXmppStream : IXmppStream
        {
            private Queue<XmlElement> sendedElements = new Queue<XmlElement>();
            private Queue<XmlElement> receivedElements = new Queue<XmlElement>();

            public Queue<XmlElement> ReceivedElements
            {
                get { return receivedElements; }
            }

            public Queue<XmlElement> SendedElements
            {
                get { return sendedElements; }
            }

            public XmlElement Read()
            {
                return receivedElements.Dequeue();
            }

            public void Write(XmlElement element)
            {
                sendedElements.Enqueue(element);
            }
        }
    }
}
