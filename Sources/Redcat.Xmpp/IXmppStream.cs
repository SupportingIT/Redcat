using Redcat.Xmpp.Xml;

namespace Redcat.Xmpp
{
    public interface IXmppStream
    {
        XmlElement Read();
        void Write(XmlElement element);
    }
}
