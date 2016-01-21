using Redcat.Xmpp.Xml;

namespace Redcat.Xmpp
{
    public class NegotiationContext
    {
        public NegotiationContext(IXmppStream stream)
        {
            Stream = stream;
        }

        public XmlElement Feature { get; set; }

        public IXmppStream Stream { get; }

        public bool IsAuthenticated { get; set; }

        public bool IsTlsEstablished { get; set; }
    }
}
