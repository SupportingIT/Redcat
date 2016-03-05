using System.Collections.Generic;

namespace Redcat.Xmpp
{
    public class RosterItem
    {
        public RosterItem()
        { }

        public string Name { get; internal set; }

        public JID Jid { get; internal set; }

        public IEnumerable<string> Groups { get; }

        public bool PendingSubscription { get; }

        public SubscriptionState SubscriptionState { get; internal set; }

        public string Version { get; }

        public override string ToString()
        {
            return $"{Name}({Jid})";
        }
    }

    public enum SubscriptionState { None, To, From, Both }
}
