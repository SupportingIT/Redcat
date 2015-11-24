using NUnit.Framework;
using Redcat.Core;
using Redcat.Xmpp.Negotiators;
using Redcat.Xmpp.Xml;
using System;

namespace Redcat.Xmpp.Tests.Negotiators
{
    [TestFixture]
    public class BindNegotiatorTests
    {
        private readonly XmlElement Bind = new XmlElement("bind", Namespaces.Bind);

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CanNegotiate_Throws_Exception_If_Parameter_Is_Null()
        {
            BindNegotiator negotiator = new BindNegotiator(new ConnectionSettings());
            negotiator.CanNegotiate(null);
        }

        [Test]
        public void CanNegotiate_Returns_True_For_Bind_Feature()
        {
            BindNegotiator negotiator = new BindNegotiator(new ConnectionSettings());            

            Assert.That(negotiator.CanNegotiate(Bind), Is.True);
        }

        [Test]
        public void Negoatiate_Sends_Correct_Iq()
        {
            ConnectionSettings settings = new ConnectionSettings();
            BindNegotiator negotiator = new BindNegotiator(settings);
            TestXmppStream stream = new TestXmppStream();
                        

            negotiator.Negotiate(stream, Bind);
            XmlElement bindRequest = stream.SendedElements.Dequeue();
        }
    }
}
