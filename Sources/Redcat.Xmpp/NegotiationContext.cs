using Redcat.Xmpp.Xml;

namespace Redcat.Xmpp
{
    public class NegotiationContext
    {
        public NegotiationContext(IXmppStream stream)
        {
            Stream = stream;
        }        

        public IXmppStream Stream { get; }

        public bool IsAuthenticated { get; set; }

        public bool IsTlsEstablished { get; set; }

        public bool IsSessionEsteblished { get; set; }

        public JID Jid { get; set; }
    }
}
