using Redcat.Core;
using System.Collections.Generic;

namespace Redcat.Xmpp
{
    public class RosterItem : Contact
    {
        public RosterItem(JID jid = null) : base(jid)
        {
            Jid = jid;
        }

        public JID Jid { get; }

        public IEnumerable<string> Groups { get; }

        public bool PendingSubscription { get; }

        public SubscriptionState SubscriptionState { get; }

        public string Version { get; }
    }

    public enum SubscriptionState { None, To, From, Both }
}
