using Redcat.Xmpp.Xml;
using System;

namespace Redcat.Xmpp
{
    public class PresenceHandler
    {
        private Action<Stanza> sendStanza;
        private PresenceStatus status;

        public PresenceHandler(Action<Stanza> sendStanza)
        {
            this.sendStanza = sendStanza;
        }

        public PresenceStatus PresenceStatus { get; private set; }

        public void SetStatus(PresenceStatus status)
        {
            PresenceStanza stanza = GetStanzaForStatus(status);
            sendStanza(stanza);
            this.status = status;
        }

        public void OnPresenceStanzaReceived(PresenceStanza stanza)
        {
            throw new NotImplementedException();
        }

        private PresenceStanza GetStanzaForStatus(PresenceStatus status)
        {
            if (status == PresenceStatus.Available) return Presence.Available();
            if (status == PresenceStatus.Away) return Presence.Available().ShowAway();
            if (status == PresenceStatus.Chat) return Presence.Available().ShowChat();
            if (status == PresenceStatus.DoNotDisturb) return Presence.Available().ShowDoNotDisturb();
            return Presence.Unavailable();
        }
    }

    public enum PresenceStatus { Unavaliable, Available, Away, Chat, DoNotDisturb }
}
