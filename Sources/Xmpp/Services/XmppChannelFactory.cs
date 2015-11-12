using System;
using Redcat.Core;
using Redcat.Core.Net;
using Redcat.Core.Communication;

namespace Redcat.Xmpp.Services
{
    public class XmppChannelFactory : IChannelFactory
    {
        private INetworkStreamFactory streamFactory;

        public XmppChannelFactory(INetworkStreamFactory streamFactory)
        {
            if (streamFactory == null) throw new ArgumentNullException(nameof(streamFactory));
            this.streamFactory = streamFactory;
        }

        public IMessageChannel CreateChannel(ConnectionSettings settings)
        {
            return new XmppChannel(streamFactory, settings);
        }
    }
}
