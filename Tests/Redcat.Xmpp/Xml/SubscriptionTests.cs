using NUnit.Framework;
using Redcat.Xmpp.Xml;

namespace Redcat.Xmpp.Tests.Xml
{
    [TestFixture]
    public class SubscriptionTests
    {
        private JID jid = "hello@world";

        [Test]
        public void Request_Returns_Subscription_Request_Stanza()
        {            
            PresenceStanza stanza = Subscription.Request(jid);

            Assert.That(stanza.IsSubscribe(), Is.True);
            Assert.That(stanza.To, Is.EqualTo(jid));
        }

        [Test]
        public void IncomingRequest_Returns_Incoming_Subscription_Request_Stanza()
        {
            PresenceStanza stanza = Subscription.IncomingRequest(jid);

            Assert.That(stanza.IsSubscribe(), Is.True);
            Assert.That(stanza.From, Is.EqualTo(jid));
        }

        [Test]
        public void IsSubscriptionRequest_Returns_True_For_Subscription_Request_Stanza()
        {
            PresenceStanza stanza = Presence.Subscribe();
            stanza.From = jid;

            Assert.That(stanza.IsSubscriptionRequest(), Is.True);
        }

        [Test]
        public void SubscriptionApprove_Returns_Subscription_Approve_Stanza()
        {
            PresenceStanza stanza = Subscription.SubscriptionApprove(jid);

            Assert.That(stanza.IsSubscribed(), Is.True);
            Assert.That(stanza.To, Is.EqualTo(jid));
        }

        [Test]
        public void IsSubscriptionApprove_Returns_True_For_Subscription_Approve_Stanza()
        {
            PresenceStanza stanza = Presence.Subscribed();

            Assert.That(stanza.IsSubscriptionApprove(), Is.True);
        }

        [Test]
        public void SubscriptionReject_Returs_Subscription_Reject_Stanza()
        {
            PresenceStanza stanza = Subscription.SubscriptionReject(jid);

            Assert.That(stanza.IsUnsubscribed(), Is.True);
            Assert.That(stanza.To, Is.EqualTo(jid));
        }

        [Test]
        public void IsSubscriptionReject_Returns_True_For_Subscription_Reject_Stanza()
        {
            PresenceStanza stanza = Presence.Unsubscribed();

            Assert.That(stanza.IsSubscriptionReject(), Is.True);
        }
    }
}
