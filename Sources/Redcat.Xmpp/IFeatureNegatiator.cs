using Redcat.Xmpp.Xml;

namespace Redcat.Xmpp
{
    public interface IFeatureNegatiator
    {
        bool CanNegotiate(NegotiationContext context, XmlElement feature);
        bool Negotiate(NegotiationContext context, XmlElement feature);
    }
}
