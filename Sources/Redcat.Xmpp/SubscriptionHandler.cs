using Redcat.Core.Channels;
using Redcat.Xmpp.Xml;
using System;
using System.Collections.Generic;

namespace Redcat.Xmpp
{
    public class SubscriptionHandler : IObserver<PresenceStanza>
    {
        private IOutputChannel<XmlElement> channel;
        private List<JID> incomingSubscriptions;
        
        public SubscriptionHandler(IOutputChannel<XmlElement> channel)
        {
            this.channel = channel;
            incomingSubscriptions = new List<JID>();
        }

        public IEnumerable<JID> IncomingSubscriptions => incomingSubscriptions;

        public void AcceptSubscription(JID subscriptionJid)
        {
            incomingSubscriptions.Remove(subscriptionJid);
            channel.Send(Subscription.SubscriptionApprove(subscriptionJid));
        }

        public void CancelSubscription(JID subscriber)
        {
            incomingSubscriptions.Remove(subscriber);
            channel.Send(Subscription.SubscriptionReject(subscriber));
        }

        public void RequestSubscription(JID subscriptionJid)
        {
            channel.Send(Subscription.Request(subscriptionJid));
        }

        public void OnCompleted()
        { }

        public void OnError(Exception error)
        { }        

        public void OnNext(PresenceStanza stanza)
        {
            if (stanza.IsSubscriptionRequest()) incomingSubscriptions.Add(stanza.From);
        }
    }
}