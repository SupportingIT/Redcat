using Redcat.Core;
using Redcat.Xmpp.Xml;

namespace Redcat.Xmpp.Negotiators
{
    public delegate XmlElement SaslAuthenticator(IXmppStream stream, ISaslCredentials settings);
}
