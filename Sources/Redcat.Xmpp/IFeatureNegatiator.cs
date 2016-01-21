using Redcat.Xmpp.Xml;

namespace Redcat.Xmpp
{
    public interface IFeatureNegatiator
    {
        bool CanNegotiate(XmlElement feature);
        bool Negotiate(NegotiationContext context);
    }
}
