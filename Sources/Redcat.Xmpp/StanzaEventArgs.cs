using Redcat.Xmpp.Xml;
using System;

namespace Redcat.Xmpp
{
    public class StanzaEventArgs<T> : EventArgs where T : Stanza
    {
        public StanzaEventArgs(T stanza)
        {
            Stanza = stanza;
        }

        public T Stanza { get; }
    }

    public class IqStanzaEventArgs : StanzaEventArgs<IqStanza>
    {
        public IqStanzaEventArgs(IqStanza iq) : base(iq)
        { }
    }

    public class PresenceStanzaEventArgs : StanzaEventArgs<PresenceStanza>
    {
        public PresenceStanzaEventArgs(PresenceStanza presence) : base(presence)
        { }
    }

    public class MessageStanzaEventArgs : StanzaEventArgs<MessageStanza>
    {
        public MessageStanzaEventArgs(MessageStanza message) : base(message)
        { }
    }
}
