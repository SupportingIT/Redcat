using Redcat.Core;

namespace Redcat.Xmpp.Negotiators
{
    public delegate void SaslAuthenticator(IXmppStream stream, ConnectionSettings settings);
}
