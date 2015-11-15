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
            IFeatureNegatiator negotiator = CreateNegotiator(true);
            initializer.Negotiators.Add(negotiator);

            RunInitializer();

            A.CallTo(() => negotiator.CanNeogatiate(A<XmlElement>._)).MustNotHaveHappened();
            A.CallTo(() => negotiator.Neogatiate(stream, A<XmlElement>._)).MustNotHaveHappened();
        }

        [Test]
        public void Uses_Negotiator_Only_Once_Per_Feature_Response()
        {
            IFeatureNegatiator negotiator = CreateNegotiator(true);
            var features = Enumerable.Range(0, 3).Select(i => new XmlElement("feature" + i)).ToArray();
            initializer.Negotiators.Add(negotiator);
            EnqueueResponse(features);

            RunInitializer();

            A.CallTo(() => negotiator.Neogatiate(stream, A<XmlElement>._)).MustHaveHappened(Repeated.Exactly.Once);
        }

        [Test]
        public void Can_Negotiate_More_Than_One_Iteration()
        {
            IFeatureNegatiator negotiator = CreateNegotiator(true, true);
            initializer.Negotiators.Add(negotiator);            
                        
            EnqueueResponse(new XmlElement("feature1"), new XmlElement("feature2"));            
            EnqueueResponse(new XmlElement("feature3"));
            RunInitializer();

            A.CallTo(() => negotiator.Neogatiate(stream, A<XmlElement>._)).MustHaveHappened(Repeated.Exactly.Twice);
        }

        [Test]
        public void End_Negotiation_If_No_Negotiators_For_Received_Features()
        {
            IFeatureNegatiator negotiator = A.Fake<IFeatureNegatiator>();
            initializer.Negotiators.Add(negotiator);
            EnqueueResponse(new XmlElement("f1"), new XmlElement("f2"));

            RunInitializer(false);

            A.CallTo(() => negotiator.Neogatiate(stream, A<XmlElement>._)).MustNotHaveHappened();
        }

        [Test]
        public void Restart_Stream_If_Negotiator_Returns_True()
        {
            IFeatureNegatiator negotiator = CreateNegotiator(true, true);
            initializer.Negotiators.Add(negotiator);
            EnqueueResponse(new XmlElement("test-feature"));

            RunInitializer();

            Assert.That(stream.SendedElements.Count, Is.EqualTo(2));
            stream.GetSentElement();
            var header = stream.GetSentElement();
            Assert.That(header.Name, Is.EqualTo("stream:stream"));
        }

        [Test]
        public void Does_Not_Restart_Stream_If_Negotiator_Returns_False()
        {
            IFeatureNegatiator negotiator = CreateNegotiator(true);
            
            initializer.Negotiators.Add(negotiator);
            EnqueueResponse(new XmlElement("test-feature"));

            RunInitializer();

            Assert.That(stream.SendedElements.Count, Is.EqualTo(1));
        }

        [Test]
        public void Negoatiates_Tls_Feature_First()
        {
            var tlsFeature = Tls.Start;
            IFeatureNegatiator tlsNegotiator = A.Fake<IFeatureNegatiator>();
            A.CallTo(() => tlsNegotiator.CanNeogatiate(tlsFeature)).Returns(true);
            IFeatureNegatiator negotiator = A.Fake<IFeatureNegatiator>();
            A.CallTo(() => negotiator.CanNeogatiate(A<XmlElement>._)).Returns(true);
            A.CallTo(() => negotiator.CanNeogatiate(tlsFeature)).Returns(false);            
            initializer.AddNegotiators(new[] { negotiator, tlsNegotiator });
            EnqueueResponse(new XmlElement("feature1"), tlsFeature, new XmlElement("feature2"));

            RunInitializer();

            A.CallTo(() => tlsNegotiator.Neogatiate(stream, tlsFeature)).MustHaveHappened();
            A.CallTo(() => negotiator.Neogatiate(stream, A<XmlElement>._)).MustNotHaveHappened();
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Throws_Exception_If_IterationLimit_Exceed()
        {
            int iterationCount = 4;
            initializer.IterationLimit = iterationCount - 1;
            IFeatureNegatiator negotiator = CreateNegotiator(true, true);
            initializer.Negotiators.Add(negotiator);
            for (int i = 0; i < iterationCount; i++) EnqueueResponse(new []{ new XmlElement("element") });

            RunInitializer(false);
        }

        private string[] invalidHeaders =
        {
            "<stream:stream from='test-domain' xmlns='invalid-xmlns' xmlns:stream='http://etherx.jabber.org/streams'>",
            //"<stream:stream from='test-domain' xmlns='jabber:client' xmlns:stream='invalid-xmlns-stream'>",
            "<invalid:name from='test-domain' xmlns='jabber:client' xmlns:stream='http://etherx.jabber.org/streams'/>",
            //"<stream:stream from='invalid-domain' xmlns='jabber:client' xmlns:stream='http://etherx.jabber.org/streams'>"
        };

        [Test]
        [ExpectedException(typeof(ProtocolViolationException))]
        public void Throws_Exception_If_Invalid_Header_Received([ValueSource("invalidHeaders")]string headerXml)
        {
            stream.EnqueueResponse(Parse(headerXml));
            EnqueueFeaturesResponse();
            
            initializer.Init(stream);
        }

        private IFeatureNegatiator CreateNegotiator(bool canNeogatiateAny, bool neogatiatesAny = false)
        {
            var negotiator = A.Fake<IFeatureNegatiator>();
            if (canNeogatiateAny) A.CallTo(() => negotiator.CanNeogatiate(A<XmlElement>._)).Returns(true);
            if (neogatiatesAny) A.CallTo(() => negotiator.Neogatiate(stream, A<XmlElement>._)).Returns(true);
            return negotiator;
        }

        private void RunInitializer(bool enqueueEmptyResponse = true)
        {
            if (enqueueEmptyResponse) EnqueueResponse();
            initializer.Init(stream);
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
    }
}
