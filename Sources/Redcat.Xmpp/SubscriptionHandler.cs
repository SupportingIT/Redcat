using Redcat.Xmpp.Xml;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Redcat.Xmpp
{
    public class SubscriptionHandler
    {
        private Action<Stanza> sendAction;
        private List<JID> incomingSubscriptions;
        
        public SubscriptionHandler(Action<Stanza> sendAction)
        {
            this.sendAction = sendAction;
            incomingSubscriptions = new List<JID>();
        }

        public IEnumerable<JID> IncomingSubscriptions => incomingSubscriptions;

        public void AcceptSubscription(JID subscriptionJid)
        {
            incomingSubscriptions.Remove(subscriptionJid);
            sendAction(Subscription.SubscriptionApprove(subscriptionJid));
        }

        public void CancelSubscription(JID subscriber)
        {
            incomingSubscriptions.Remove(subscriber);
            sendAction(Subscription.SubscriptionReject(subscriber));
        }

        public void RequestSubscription(JID subscriptionJid)
        {
            sendAction(Subscription.Request(subscriptionJid));
        }             

        public void OnPresenceStanzaReceived(PresenceStanza stanza)
        {
            if (stanza.IsSubscriptionRequest()) incomingSubscriptions.Add(stanza.From);
        }

        public void OnRosterUpdated(IEnumerable<RosterItem> roster)
        {
            foreach (var item in roster.Where(i => i.SubscriptionState == SubscriptionState.To))
            {
                incomingSubscriptions.Add(item.Jid);
            }
        }
    }
}