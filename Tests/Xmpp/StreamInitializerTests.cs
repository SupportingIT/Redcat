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
        public void Sends_Valid_Stream_Header()
        {
            RunInitializer();

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
            var feature = new XmlElement("feature1");
            EnqueueResponse(feature);

            RunInitializer();

            A.CallTo(() => negotiators[0].Neogatiate(stream, feature)).MustNotHaveHappened();
            A.CallTo(() => negotiators[1].Neogatiate(stream, feature)).MustHaveHappened();
            A.CallTo(() => negotiators[2].Neogatiate(stream, feature)).MustNotHaveHappened();
        }

        [Test]
        public void Does_Not_Uses_Neogatiators_If_No_Features_Received()
        {
            IFeatureNegatiator negotiator = A.Fake<IFeatureNegatiator>();
            A.CallTo(() => negotiator.CanNeogatiate(A<XmlElement>._)).Returns(true);
            initializer.Negotiators.Add(negotiator);

            RunInitializer();

            A.CallTo(() => negotiator.CanNeogatiate(A<XmlElement>._)).MustNotHaveHappened();
            A.CallTo(() => negotiator.Neogatiate(stream, A<XmlElement>._)).MustNotHaveHappened();
        }

        [Test]
        public void Uses_Negotiator_Only_Once_Per_Feature_Response()
        {
            IFeatureNegatiator negotiator = A.Fake<IFeatureNegatiator>();
            A.CallTo(() => negotiator.CanNeogatiate(A<XmlElement>._)).Returns(true);
            var features = Enumerable.Range(0, 3).Select(i => new XmlElement("feature" + i)).ToArray();
            initializer.Negotiators.Add(negotiator);
            EnqueueResponse(features);

            RunInitializer();

            A.CallTo(() => negotiator.Neogatiate(stream, A<XmlElement>._)).MustHaveHappened(Repeated.Exactly.Once);
        }

        [Test]
        public void Can_Negotiate_More_Than_One_Iteration()
        {
            IFeatureNegatiator negotiator = A.Fake<IFeatureNegatiator>();
            A.CallTo(() => negotiator.CanNeogatiate(A<XmlElement>._)).Returns(true);
            initializer.Negotiators.Add(negotiator);            
                        
            EnqueueResponse(new XmlElement("feature1"), new XmlElement("feature2"));            
            EnqueueResponse(new XmlElement("feature3"));
            RunInitializer();

            A.CallTo(() => negotiator.Neogatiate(stream, A<XmlElement>._)).MustHaveHappened(Repeated.Exactly.Twice);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Throws_Exception_If_IterationLimit_Exceed()
        {
            int iterationCount = 4;
            initializer.IterationLimit = iterationCount - 1;
            IFeatureNegatiator negotiator = A.Fake<IFeatureNegatiator>();
            A.CallTo(() => negotiator.CanNeogatiate(A<XmlElement>._)).Returns(true);
            initializer.Negotiators.Add(negotiator);
            for (int i = 0; i < iterationCount; i++) EnqueueResponse(new []{ new XmlElement("element") });

            RunInitializer(false);
        }

        private string[] invalidHeaders =
        {
            "<stream:stream from='test-domain' xmlns='invalid-xmlns' xmlns:stream='http://etherx.jabber.org/streams'>",
            "<stream:stream from='test-domain' xmlns='jabber:client' xmlns:stream='invalid-xmlns-stream'>",
            "<invalid:name from='test-domain' xmlns='jabber:client' xmlns:stream='http://etherx.jabber.org/streams'/>",
            //"<stream:stream from='invalid-domain' xmlns='jabber:client' xmlns:stream='http://etherx.jabber.org/streams'>"
        };

        [Test]
        [ExpectedException(typeof(ProtocolViolationException))]
        public void Throws_Exception_If_Invalid_Header_Received([ValueSource("invalidHeaders")]string headerXml)
        {
            stream.EnqueueResponse(Parse(headerXml));
            EnqueueFeaturesResponse();
            
            initializer.Start(stream);
        }

        private void RunInitializer(bool enqueueEmptyResponse = true)
        {
            if (enqueueEmptyResponse) EnqueueResponse();
            initializer.Start(stream);
        }

        private void EnqueueResponse(params XmlElement[] features)
        {
            EnqueueResponseHeader();
            EnqueueFeaturesResponse(features);
        }

        private void EnqueueResponseHeader()
        {
            stream.EnqueueResponse(StreamHeader.CreateClientHeader(settings.Domain));
        }

        private void EnqueueFeaturesResponse(params XmlElement[] features)
        {
            XmlElement featuresElement = new XmlElement("stream:features");
            foreach (var feature in features) featuresElement.Childs.Add(feature);
            stream.EnqueueResponse(featuresElement);
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
