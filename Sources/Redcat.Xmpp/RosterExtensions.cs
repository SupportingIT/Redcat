using Redcat.Core;
using Redcat.Xmpp.Xml;

namespace Redcat.Xmpp
{
    public static class RosterExtensions
    {
        public static void SendRosterRequest(this ICommunicator communicator, JID from = null)
        {
            IqStanza request = Roster.Get(from);
            communicator.Send(request);
        }

        public static void SendRosterAdd(this ICommunicator communicator, JID itemJid)
        {
            IqStanza request = Roster.Set(itemJid);
            communicator.Send(request);
        }
    }
}
