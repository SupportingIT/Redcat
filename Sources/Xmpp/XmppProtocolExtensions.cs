using Redcat.Core;
using Redcat.Core.Net;
using Redcat.Core.Services;
using Redcat.Xmpp.Services;
using System;

namespace Redcat.Xmpp
{
    public static class XmppProtocolExtensions
    {
        public static void AddXmppExtension(this Communicator communicator, Func<ISocket> socketFactory)
        {
            communicator.AddExtension("xmpp", c => {
                var factory = new XmppChannelFactory(socketFactory);
                c.Add<IChannelFactory>(factory);
            });
        }
    }
}
