using Redcat.Core;
using Redcat.Xmpp.Xml;

namespace Redcat.Xmpp
{
    public static class RosterExtensions
    {
        public static void SendRosterRequest(this ICommunicator communicator)
        {
            IqStanza request = Roster.Get(null);
            communicator.Send(request);
        }
    }
}
