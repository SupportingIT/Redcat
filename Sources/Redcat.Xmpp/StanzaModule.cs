using Redcat.Core.Channels;
using Redcat.Xmpp.Xml;

namespace Redcat.Xmpp
{
    public class StanzaModule
    {
        private IqStanzaHandler iqHandler;
        private PresenceStanzaHandler presenceHandler;
        private MessageStanzaHandler messageHandler;

        public StanzaModule(IqStanzaHandler iqHandler, PresenceStanzaHandler presenceHandler, MessageStanzaHandler messageHandler)
        {
            this.iqHandler = iqHandler;
            this.presenceHandler = presenceHandler;
            this.messageHandler = messageHandler;
        }

        public void OnXmlElementReceived(object sender, ChannelMessageEventArgs<XmlElement> args)
        {
            if (args.Message is IqStanza && iqHandler != null) iqHandler((IqStanza)args.Message);
            if (args.Message is PresenceStanza && presenceHandler != null) presenceHandler((PresenceStanza)args.Message);
            if (args.Message is MessageStanza && messageHandler != null) messageHandler((MessageStanza)args.Message);
        }
    }

    public delegate void IqStanzaHandler(IqStanza stanza);
    public delegate void PresenceStanzaHandler(PresenceStanza stanza);
    public delegate void MessageStanzaHandler(MessageStanza stanza);
}
