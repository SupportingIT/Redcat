using System;
using System.Linq;
using System.Net;
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
        private NegotiationContext context;
        private StreamInitializer initializer;
        private TestXmppStream stream;

        [SetUp]
        public void Setup()
        {            
            stream = new TestXmppStream();
            settings = new ConnectionSettings { Domain = "test-domain" };
            initializer = new StreamInitializer(settings);
            context = new NegotiationContext(stream);
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
            initializer.AddNegotiators(negotiators);            
            var feature = new XmlElement("feature1");
            A.CallTo(() => negotiators[1].CanNegotiate(context, feature)).Returns(true);
            EnqueueResponse(true, feature);

            RunInitializer(false);

            A.CallTo(() => negotiators[0].Negotiate(context, feature)).MustNotHaveHappened();
            A.CallTo(() => negotiators[1].Negotiate(context, feature)).MustHaveHappened();
            A.CallTo(() => negotiators[2].Negotiate(context, feature)).MustNotHaveHappened();
        }

        [Test]
        public void Does_Not_Uses_Neogatiators_If_No_Features_Received()
        {
            IFeatureNegatiator negotiator = CreateNegotiator(true);
            initializer.Negotiators.Add(negotiator);

            RunInitializer();

            A.CallTo(() => negotiator.CanNegotiate(context, A<XmlElement>._)).MustNotHaveHappened();
            A.CallTo(() => negotiator.Negotiate(context, A<XmlElement>._)).MustNotHaveHappened();
        }

        [Test]
        public void Uses_Negotiator_Only_Once_Per_Feature_Response()
        {
            IFeatureNegatiator negotiator = CreateNegotiator(true);
            var features = Enumerable.Range(0, 3).Select(i => new XmlElement("feature" + i)).ToArray();
            initializer.Negotiators.Add(negotiator);
            EnqueueResponse(true, features);

            RunInitializer(false);

            A.CallTo(() => negotiator.Negotiate(context, A<XmlElement>._)).MustHaveHappened(Repeated.Exactly.Once);
        }

        [Test]
        public void Can_Negotiate_More_Than_One_Iteration()
        {
            IFeatureNegatiator negotiator = CreateNegotiator(true, true);
            initializer.Negotiators.Add(negotiator);            
                        
            EnqueueResponse(true, new XmlElement("feature1"), new XmlElement("feature2"));            
            EnqueueResponse(true, new XmlElement("feature3"));
            RunInitializer();

            A.CallTo(() => negotiator.Negotiate(context, A<XmlElement>._)).MustHaveHappened(Repeated.Exactly.Twice);
        }

        [Test]
        public void End_Negotiation_If_No_Negotiators_For_Received_Features()
        {
            IFeatureNegatiator negotiator = A.Fake<IFeatureNegatiator>();
            initializer.Negotiators.Add(negotiator);
            EnqueueResponse(true, new XmlElement("f1"), new XmlElement("f2"));

            RunInitializer(false);

            A.CallTo(() => negotiator.Negotiate(context, A<XmlElement>._)).MustNotHaveHappened();
        }

        [Test]
        public void Restart_Stream_If_Negotiator_Returns_True()
        {
            IFeatureNegatiator negotiator = CreateNegotiator(true, true);
            initializer.Negotiators.Add(negotiator);
            EnqueueResponse(true, new XmlElement("test-feature"));

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
            EnqueueResponse(false, new XmlElement("test-feature"));

            RunInitializer();

            Assert.That(stream.SendedElements.Count, Is.EqualTo(1));
        }

        [Test]
        public void Negoatiates_Tls_Feature_First()
        {
            IFeatureNegatiator tlsNegotiator = A.Fake<IFeatureNegatiator>();
            
            A.CallTo(() => tlsNegotiator.CanNegotiate(context, A<XmlElement>._)).Returns(true);
            A.CallTo(() => tlsNegotiator.Negotiate(context, A<XmlElement>._)).Returns(true);
            IFeatureNegatiator negotiator = A.Fake<IFeatureNegatiator>();
            A.CallTo(() => negotiator.CanNegotiate(context, A<XmlElement>._)).Returns(true);
            A.CallTo(() => negotiator.CanNegotiate(context, A<XmlElement>._)).Returns(false);            
            initializer.AddNegotiators(new[] { negotiator, tlsNegotiator });
            EnqueueResponse(true, new XmlElement("feature1"), Tls.Start, new XmlElement("feature2"));

            RunInitializer();

            A.CallTo(() => tlsNegotiator.Negotiate(context, A<XmlElement>._)).MustHaveHappened();
            A.CallTo(() => negotiator.Negotiate(context, A<XmlElement>._)).MustNotHaveHappened();
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Throws_Exception_If_IterationLimit_Exceed()
        {
            int iterationCount = 4;
            initializer.IterationLimit = iterationCount - 1;
            IFeatureNegatiator negotiator = CreateNegotiator(true, true);
            initializer.Negotiators.Add(negotiator);
            for (int i = 0; i < iterationCount; i++) EnqueueResponse(true, new []{ new XmlElement("element") });

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
            if (canNeogatiateAny) A.CallTo(() => negotiator.CanNegotiate(context, A<XmlElement>._)).Returns(true);
            if (neogatiatesAny) A.CallTo(() => negotiator.Negotiate(context, A<XmlElement>._)).Returns(true);
            return negotiator;
        }

        private void RunInitializer(bool enqueueHeader = true, bool enqueueEmptyFeatures = true)
        {
            if (enqueueHeader) EnqueueResponseHeader();
            if (enqueueEmptyFeatures) EnqueueFeaturesResponse();
            initializer.Init(stream);
        }

        private void EnqueueResponse(bool includeHeader, params XmlElement[] features)
        {
            if (includeHeader) EnqueueResponseHeader();
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
