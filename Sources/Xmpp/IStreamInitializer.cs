using Redcat.Core;

namespace Redcat.Xmpp
{
    public interface IStreamInitializer
    {
        void Init(IXmppStream stream);
    }
}
