using System;
using Redcat.Core;
using Redcat.Core.Services;
using Redcat.Core.Net;
using System.IO;

namespace Redcat.Xmpp.Services
{
    public class XmppChannelFactory : IChannelFactory
    {
        private Func<ISocket> socketFactory;
        private Func<Stream, Stream> tlsContextFactory;

        public XmppChannelFactory(Func<ISocket> socketFactory, Func<Stream, Stream> tlsContextFactory)
        {
            if (socketFactory == null) throw new ArgumentNullException(nameof(socketFactory));
            this.socketFactory = socketFactory;
            this.tlsContextFactory = tlsContextFactory;
        }

        public IMessageChannel CreateChannel(ConnectionSettings settings)
        {
            return new XmppChannel(socketFactory.Invoke(), settings) { TlsContextFactory = tlsContextFactory };
        }
    }
}
