using Redcat.Core;

namespace Redcat.Xmpp
{
    public class RosterItem : Contact
    {
        public RosterItem(JID jid) : base(jid)
        {
            Jid = jid;
        }

        public JID Jid { get; }
    }
}
