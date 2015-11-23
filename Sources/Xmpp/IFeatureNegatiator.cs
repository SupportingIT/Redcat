using Redcat.Xmpp.Xml;

namespace Redcat.Xmpp
{
    public interface IFeatureNegatiator
    {
        bool CanNegotiate(XmlElement feature);
        bool Negotiate(IXmppStream stream, XmlElement feature);
    }
}
