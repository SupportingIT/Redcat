using FakeItEasy;
using NUnit.Framework;
using Redcat.Xmpp.Xml;
using System;

namespace Redcat.Xmpp.Tests
{
    [TestFixture]
    public class SubscriptionHandlerTests
    {
        private SubscriptionHandler handler;
        private Action<Stanza> sendAction;
        private PresenceStanza stanza;

        [SetUp]
        public void SetUp()
        {
            sendAction = A.Fake<Action<Stanza>>();
            handler = new SubscriptionHandler(sendAction);
            A.CallTo(() => sendAction.Invoke(A<Stanza>._)).Invokes(c => {
                stanza = (PresenceStanza)c.GetArgument<Stanza>(0);
            });
        }

        [Test]
        public void RequestSubscription_Submits_Subscription_Request()
        {            
            JID subsctiption = "subscription@redcat.com";

            handler.RequestSubscription(subsctiption);

            Assert.That(stanza.IsSubscribe(), Is.True);
            Assert.That(stanza.To, Is.EqualTo(subsctiption));
        }

        [Test]
        public void SubscriptionRequest_Stanza_Adds_IncomingSubscriptions_Jid()
        {
            SubscriptionHandler handler = new SubscriptionHandler(sendAction);
            JID[] subscribers = { "user1@redcat", "user2@redcat" };

            handler.OnPresenceStanzaReceived(Subscription.IncomingRequest(subscribers[0]));
            handler.OnPresenceStanzaReceived(Subscription.IncomingRequest(subscribers[1]));

            CollectionAssert.Contains(handler.IncomingSubscriptions, subscribers[0]);
            CollectionAssert.Contains(handler.IncomingSubscriptions, subscribers[1]);
        }

        [Test]
        public void AcceptSubscription_Sends_SubscriptionApprove_Stanza()
        {            
            JID subscriber = "user@redcat.org";
            handler.OnPresenceStanzaReceived(Subscription.IncomingRequest(subscriber));

            handler.AcceptSubscription(subscriber);

            CollectionAssert.DoesNotContain(handler.IncomingSubscriptions, subscriber);
            Assert.That(stanza.IsSubscriptionApprove(), Is.True);
        }

        [Test]
        public void CancelSubscription_Sends_SubscriptionApprove_Stanza()
        {
            JID subscriber = "user@redcat.org";
            handler.OnPresenceStanzaReceived(Subscription.IncomingRequest(subscriber));

            handler.CancelSubscription(subscriber);

            CollectionAssert.DoesNotContain(handler.IncomingSubscriptions, subscriber);
            Assert.That(stanza.IsSubscriptionReject(), Is.True);
        }
    }
}
