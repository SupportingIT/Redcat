using Redcat.Core;
using Redcat.Xmpp.Xml;
using System;
using System.Collections.Generic;

namespace Redcat.Xmpp
{
    public class SubscriptionHandler : IObserver<PresenceStanza>, IObserver<ContactCommand>, IObservable<PresenceStanza>
    {
        private ICollection<IObserver<PresenceStanza>> stanzaObservers;
        private List<JID> incomingSubscriptions;

        public SubscriptionHandler()
        {
            stanzaObservers = new List<IObserver<PresenceStanza>>();
            incomingSubscriptions = new List<JID>();
        }

        public IEnumerable<JID> IncomingSubscriptions => incomingSubscriptions;

        public void AcceptSubscription(JID subscriptionJid)
        {
            incomingSubscriptions.Remove(subscriptionJid);
            stanzaObservers.OnNext(Subscription.SubscriptionApprove(subscriptionJid));
        }

        public void CancelSubscription(JID subscriber)
        {
            incomingSubscriptions.Remove(subscriber);
            stanzaObservers.OnNext(Subscription.SubscriptionReject(subscriber));
        }

        public void RequestSubscription(JID subscriptionJid)
        {
            stanzaObservers.OnNext(Subscription.Request(subscriptionJid));
        }

        public void OnCompleted()
        { }

        public void OnError(Exception error)
        { }

        public void OnNext(ContactCommand value)
        {
            throw new NotImplementedException();
        }

        public void OnNext(PresenceStanza stanza)
        {
            if (stanza.IsSubscriptionRequest()) incomingSubscriptions.Add(stanza.From);
        }

        public IDisposable Subscribe(IObserver<PresenceStanza> observer)
        {
            return stanzaObservers.Subscribe(observer);
        }
    }
}
