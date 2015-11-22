using System;
using Redcat.Core;
using Redcat.Core.Net;
using Redcat.Core.Communication;
using Redcat.Xmpp.Negotiators;

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

        public IChannel CreateChannel(ConnectionSettings settings)
        {
            StreamInitializer initializer = new StreamInitializer(settings);
            initializer.Negotiators.Add(CreateSaslNegotiator());
            XmppChannel channel = null;
            initializer.Negotiators.Add(new TlsNegotiator(channel.SetTlsStream));
            
            return channel;
        }

        private SaslNegotiator CreateSaslNegotiator()
        {
            SaslNegotiator sasl = new SaslNegotiator();
            sasl.AddAuthenticator("PLAIN", Authenticators.Plain);
            return sasl;
        }
    }    
}
