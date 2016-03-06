using System;
using System.Collections.Generic;

namespace Redcat.Xmpp
{
    public class RosterItem
    {
        private SubscriptionHandler handler;

        public RosterItem()
        { }

        internal RosterItem(SubscriptionHandler handler)
        {
            this.handler = handler;
        }

        public string Name { get; set; }

        public JID Jid { get; set; }

        public IEnumerable<string> Groups { get; }

        public bool PendingSubscription { get; }

        public SubscriptionState SubscriptionState { get; internal set; }

        public string Version { get; }

        public void ApproveSubscription()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return $"{Name}({Jid})";
        }
    }

    public enum SubscriptionState { None, To, From, Both }
}
