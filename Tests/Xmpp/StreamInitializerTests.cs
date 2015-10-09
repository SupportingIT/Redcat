using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FakeItEasy;
using NUnit.Framework;
using Redcat.Core;
using Redcat.Xmpp.Parsing;
using Redcat.Xmpp.Xml;

namespace Redcat.Xmpp.Tests
{
    [TestFixture]
    public class StreamInitializerTests
    {
        private ConnectionSettings settings;
        private StreamInitializer initializer;
        private TestXmppStream stream;

        [SetUp]
        public void Setup()
        {
            stream = new TestXmppStream();
            settings = new ConnectionSettings { Domain = "test-domain" };
            initializer = new StreamInitializer(settings);
        }

        [Test]
        public void Start_Sends_Valid_Stream_Header()
        {
            RunInitializer(initializer, stream);

            var header = stream.GetSentElement();

            Assert.That(header.Name, Is.EqualTo("stream:stream"));
            Assert.That(header.Xmlns, Is.EqualTo(Namespaces.JabberClient));
            Assert.That(header.GetAttributeValue<string>("xmlns:stream"), Is.EqualTo(Namespaces.Streams));
            Assert.That(header.GetAttributeValue<JID>("to"), Is.EqualTo((JID)settings.Domain));
        }

        [Test]
        public void Uses_Correct_Negotiator_For_Feature()
        {
            var negotiators = A.CollectionOfFake<IFeatureNegatiator>(3);
            A.CallTo(() => negotiators[1].CanNeogatiate(A<XmlElement>._)).Returns(true);
            initializer.AddNegotiators(negotiators);

            XmlElement features = new XmlElement("stream:features");
            features.Childs.Add(new XmlElement("feature1"));
            stream.EnqueueResponse(StreamHeader.CreateClientHeader(settings.Domain));
            stream.EnqueueResponse(features);

            RunInitializer(initializer, stream);
            
            A.CallTo(() => negotiators[0].Neogatiate(stream, features.Childs.First())).MustNotHaveHappened();
            A.CallTo(() => negotiators[1].Neogatiate(stream, features.Childs.First())).MustHaveHappened();
            A.CallTo(() => negotiators[2].Neogatiate(stream, features.Childs.First())).MustNotHaveHappened();
        }

        private void RunInitializer(StreamInitializer initializer, IXmppStream stream)
        {
            try { Task.Factory.StartNew(() => initializer.Start(stream)).Wait(); }
            catch (AggregateException) { }
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
            stream.EnqueueResponse(Parse(headerXml));
            
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

            public void EnqueueResponse(XmlElement response)
            {
                receivedElements.Enqueue(response);
            }

            public XmlElement GetSentElement()
            {
                return sendedElements.Dequeue();
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
