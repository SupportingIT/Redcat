using System;
using Redcat.Core;
using Redcat.Core.Net;
using Redcat.Core.Communication;

namespace Redcat.Xmpp.Services
{
    public class XmppChannelFactory : IChannelFactory
    {
        private IStreamInitializerFactory initializerFactory;
        private INetworkStreamFactory streamFactory;

        public XmppChannelFactory(INetworkStreamFactory streamFactory, IStreamInitializerFactory initializerFactory)
        {
            if (streamFactory == null) throw new ArgumentNullException(nameof(streamFactory));
            this.initializerFactory = initializerFactory;
            this.streamFactory = streamFactory;
        }

        public IMessageChannel CreateChannel(ConnectionSettings settings)
        {
            return new XmppChannel(initializerFactory.CreateInitializer(settings), streamFactory, settings);
        }
    }    
}
