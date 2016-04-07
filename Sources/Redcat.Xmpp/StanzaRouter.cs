using Redcat.Core.Channels;
using Redcat.Xmpp.Xml;
using System.Threading;

namespace Redcat.Xmpp
{
    public class StanzaRouter
    {
        private IqStanzaHandler iqHandler;
        private PresenceStanzaHandler presenceHandler;
        private MessageStanzaHandler messageHandler;

        public StanzaRouter(IqStanzaHandler iqHandler, PresenceStanzaHandler presenceHandler, MessageStanzaHandler messageHandler)
        {
            this.iqHandler = iqHandler;
            this.presenceHandler = presenceHandler;
            this.messageHandler = messageHandler;
        }

        public SynchronizationContext SyncContext { get; set; }

        public void OnXmlElementReceived(object sender, ChannelMessageEventArgs<XmlElement> args)
        {
            if (args.Message is IqStanza) OnIqReceived((IqStanza)args.Message);
            if (args.Message is PresenceStanza) OnPresenceReceived((PresenceStanza)args.Message);
            if (args.Message is MessageStanza) OnMessageReceived((MessageStanza)args.Message);
        }

        private void OnIqReceived(IqStanza iq)
        {
            if (iqHandler == null) return;
            if (SyncContext != null) SyncContext.Send(s => iqHandler(iq), null);
            else iqHandler(iq);
        }

        private void OnPresenceReceived(PresenceStanza presence)
        {
            if (presenceHandler == null) return;
            if (SyncContext != null) SyncContext.Send(s => presenceHandler(presence), null);
            else presenceHandler(presence);
        }

        private void OnMessageReceived(MessageStanza message)
        {
            if (messageHandler == null) return;
            if (SyncContext != null) SyncContext.Send(s => messageHandler(message), null);
            else messageHandler(message);
        }
    }

    public delegate void IqStanzaHandler(IqStanza stanza);
    public delegate void PresenceStanzaHandler(PresenceStanza stanza);
    public delegate void MessageStanzaHandler(MessageStanza stanza);
}
