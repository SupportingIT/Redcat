using Redcat.Core;
using Redcat.Core.Communication;
using Redcat.Core.Service;
using Redcat.Xmpp.Communication;

namespace Redcat.Xmpp
{
    public static class XmppProtocolExtensions
    {
        public static void AddXmppExtension(this Communicator communicator)
        {
            communicator.AddExtension("xmpp", c => {
                c.TryAddSingleton<IChannelFactory, XmppChannelFactory>();
                c.TryAddSingleton<IStreamInitializerFactory, StreamInitializerFactory>();
            });
        }
    }
}
