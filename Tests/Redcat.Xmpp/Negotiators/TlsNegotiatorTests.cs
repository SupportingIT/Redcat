using FakeItEasy;
using NUnit.Framework;
using Redcat.Xmpp.Negotiators;
using Redcat.Xmpp.Xml;
using System;
using System.Linq;
using System.Net;

namespace Redcat.Xmpp.Tests.Negotiators
{
    [TestFixture]
    public class TlsNegotiatorTests
    {
        private Action setTlsContext;
        private NegotiationContext context;
        private TestXmppStream stream;
        private TlsNegotiator negotiator;

        [SetUp]
        public void Setup()
        {
            setTlsContext = A.Fake<Action>();
            stream = new TestXmppStream();
            negotiator = new TlsNegotiator(setTlsContext);
            context = new NegotiationContext(stream);
        }

        [Test]
        public void CanNegotiate_Returns_True_For_Tls_Feature()
        {
            var feature = Tls.Start;
            Assert.That(negotiator.CanNegotiate(context, feature), Is.True);
        }

        [Test]
        public void CanNegotiate_Returns_False_For_Non_Tls_Feature()
        {
            var features = new[] { new XmlElement("feature1", Namespaces.Tls), new XmlElement("starttls", "ns") };                        
            Assert.That(features.Any(f => negotiator.CanNegotiate(context, f)), Is.False);
        }

        [Test]
        public void CanNegoatiate_Returns_False_If_IsTlsEstablished_True()
        {
            context.IsTlsEstablished = true;
            Assert.That(negotiator.CanNegotiate(context, Tls.Start), Is.False);
        }

        [Test]
        public void Negotiate_Sends_Starttls_Element()
        {            
            stream.EnqueueResponse(Tls.Proceed);
         
            negotiator.Negotiate(context, Tls.Start);

            var sended = stream.GetSentElement();
            Assert.That(sended, Is.EqualTo(Tls.Start));
        }

        static XmlElement[] invalidResponses = new[] 
        {
            new XmlElement("proceed", "invalid-namespace"),
            new XmlElement("invalid-name", Namespaces.Tls)
        };

        [Test]
        [ExpectedException(typeof(ProtocolViolationException))]
        public void Negotiate_Throws_Exception_If_Response_Invalid([ValueSource("invalidResponses")] XmlElement response)
        {
            stream.EnqueueResponse(response);

            negotiator.Negotiate(context, Tls.Start);
        }

        [Test]
        public void Neogatiate_Calls_Tls_Action_And_Returns_True_If_Server_Response_Valid()
        {
            stream.EnqueueResponse(Tls.Proceed);
            
            bool restartStream = negotiator.Negotiate(context, Tls.Start);

            A.CallTo(() => setTlsContext.Invoke()).MustHaveHappened();
            Assert.That(restartStream, Is.True);
        }
    }
}
