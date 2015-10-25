using Redcat.Core;
using Redcat.Core.Net;
using Redcat.Core.Services;
using Redcat.Xmpp.Services;
using System;
using System.IO;

namespace Redcat.Xmpp
{
    public static class XmppProtocolExtensions
    {
        public static void AddXmppExtension(this Communicator communicator, Func<ISocket> socketFactory, Func<Stream, Stream> tlsContextFactory)
        {
            communicator.AddExtension("xmpp", c => {
                var factory = new XmppChannelFactory(socketFactory, tlsContextFactory);
                c.Add<IChannelFactory>(factory);
            });
        }
    }
}
