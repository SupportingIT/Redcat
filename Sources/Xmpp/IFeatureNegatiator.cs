using Redcat.Xmpp.Xml;

namespace Redcat.Xmpp
{
    public interface IFeatureNegatiator
    {
        bool CanNeogatiate(XmlElement feature);
        bool Neogatiate(IXmppStream stream, XmlElement feature);
    }
}
