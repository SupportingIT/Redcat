using NUnit.Framework;
using Redcat.Core;
using Redcat.Xmpp.Negotiators;
using Redcat.Xmpp.Xml;
using System;
using System.Linq;

namespace Redcat.Xmpp.Tests.Negotiators
{
    [TestFixture]
    public class BindNegotiatorTests
    {
        private readonly XmlElement Bind = new XmlElement("bind", Namespaces.Bind);
        private readonly JID userJid = "user-jid@domain.com/some-resource";

        private ConnectionSettings settings;
        private BindNegotiator negotiator;
        private NegotiationContext context;
        private TestXmppStream stream;

        [SetUp]
        public void SetUp()
        {
            settings = new ConnectionSettings();
            negotiator = new BindNegotiator(settings);
            stream = new TestXmppStream();
            stream.EnqueueResponse(CreateBindResponse());
            context = new NegotiationContext(stream) { Feature = Bind };
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CanNegotiate_Throws_Exception_If_Parameter_Is_Null()
        {
            negotiator.CanNegotiate(null);
        }

        [Test]
        public void CanNegotiate_Returns_True_For_Bind_Feature()
        {
            Assert.That(negotiator.CanNegotiate(Bind), Is.True);
        }

        [Test]
        public void Negoatiate_Sends_Correct_Empty_Resource_Bind()
        {
            negotiator.Negotiate(context);
            XmlElement bindRequest = stream.SendedElements.Dequeue();
            XmlElement bind = bindRequest.Childs.Single();

            Assert.That(bindRequest.Name, Is.EqualTo("iq"));
            Assert.That(bind.Name, Is.EqualTo("bind"));
            Assert.That(bind.Xmlns, Is.EqualTo(Namespaces.Bind));            
        }

        [Test]
        public void Negoatiate_Sends_Correct_Resource_Bind_With_Given_Value()
        {
            string resource = "some-res";
            settings.Resource(resource);

            negotiator.Negotiate(context);
            XmlElement bindRequest = stream.SendedElements.Dequeue();
            XmlElement res = bindRequest.Child("bind").Childs.Single();

            Assert.That(res.Name, Is.EqualTo("resource"));
            Assert.That(res.Value, Is.EqualTo(resource));
        }

        [Test]
        public void Negoatiate_Sets_User_Jid_According_Value_In_Response()
        {
            negotiator.Negotiate(context);

            Assert.That(settings.UserJid(), Is.EqualTo(userJid));
        }

        private XmlElement CreateBindResponse()
        {
            IqStanza response = Iq.Set();
            response.AddChild(new XmlElement("bind", Namespaces.Bind));
            response.Child("bind").AddChild(new XmlElement("jid") { Value = userJid });
            return response;
        }
    }
}
