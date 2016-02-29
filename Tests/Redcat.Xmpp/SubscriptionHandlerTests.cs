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
        private IObserver<PresenceStanza> observer;
        private PresenceStanza stanza;

        [SetUp]
        public void SetUp()
        {
            handler = new SubscriptionHandler();
            observer = A.Fake<IObserver<PresenceStanza>>();
            handler.Subscribe(observer);
            A.CallTo(() => observer.OnNext(A<PresenceStanza>._)).Invokes(c =>
            {
                stanza = c.GetArgument<PresenceStanza>(0);
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
            SubscriptionHandler handler = new SubscriptionHandler();
            JID[] subscribers = { "user1@redcat", "user2@redcat" };

            handler.OnNext(Subscription.IncomingRequest(subscribers[0]));
            handler.OnNext(Subscription.IncomingRequest(subscribers[1]));

            CollectionAssert.Contains(handler.IncomingSubscriptions, subscribers[0]);
            CollectionAssert.Contains(handler.IncomingSubscriptions, subscribers[1]);
        }

        [Test]
        public void AcceptSubscription_Sends_SubscriptionApprove_Stanza()
        {            
            JID subscriber = "user@redcat.org";
            handler.OnNext(Subscription.IncomingRequest(subscriber));

            handler.AcceptSubscription(subscriber);

            CollectionAssert.DoesNotContain(handler.IncomingSubscriptions, subscriber);
            Assert.That(stanza.IsSubscriptionApprove(), Is.True);
        }

        [Test]
        public void CancelSubscription_Sends_SubscriptionApprove_Stanza()
        {
            JID subscriber = "user@redcat.org";
            handler.OnNext(Subscription.IncomingRequest(subscriber));

            handler.CancelSubscription(subscriber);

            CollectionAssert.DoesNotContain(handler.IncomingSubscriptions, subscriber);
            Assert.That(stanza.IsSubscriptionReject(), Is.True);
        }
    }
}
