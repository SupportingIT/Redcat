using System;
using Redcat.Core;
using Redcat.Core.Services;
using Redcat.Core.Net;

namespace Redcat.Xmpp.Services
{
    public class XmppChannelFactory : IChannelFactory
    {
        private Func<ISocket> socketFactory;

        public XmppChannelFactory(Func<ISocket> socketFactory)
        {
            if (socketFactory == null) throw new ArgumentNullException("socketFactory");
            this.socketFactory = socketFactory;
        }

        public IMessageChannel CreateChannel(ConnectionSettings settings)
        {
            return new XmppChannel(socketFactory(), settings);
        }
    }
}
