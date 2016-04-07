namespace Redcat.Xmpp.Xml
{
    public static class Subscription
    {
        public static PresenceStanza Request(JID subscribtionJid)
        {
            PresenceStanza stanza = Presence.Subscribe();            
            stanza.To = subscribtionJid;
            return stanza;
        }

        public static PresenceStanza IncomingRequest(JID subscriberJid)
        {
            PresenceStanza stanza = Presence.Subscribe();
            stanza.From = subscriberJid;
            return stanza;
        }

        public static bool IsSubscriptionRequest(this PresenceStanza stanza)
        {
            return stanza.IsSubscribe();
        }

        public static PresenceStanza SubscriptionApprove(JID subscriberJid)
        {
            PresenceStanza stanza = Presence.Subscribed();
            stanza.To = subscriberJid;
            return stanza;
        }

        public static bool IsSubscriptionApprove(this PresenceStanza stanza)
        {
            return stanza.IsSubscribed();
        }

        public static PresenceStanza SubscriptionReject(JID subscriber)
        {
            PresenceStanza stanza = Presence.Unsubscribed();
            stanza.To = subscriber;
            return stanza;
        }

        public static bool IsSubscriptionReject(this PresenceStanza stanza)
        {
            return stanza.IsUnsubscribed();
        }
    }
}
