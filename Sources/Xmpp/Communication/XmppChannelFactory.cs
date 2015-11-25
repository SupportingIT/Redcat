using Redcat.Core;
using Redcat.Core.Communication;
using Redcat.Xmpp.Negotiators;
using StreamChannelFactory = Redcat.Core.Communication.IChannelFactory<Redcat.Core.Communication.IStreamChannel>;

namespace Redcat.Xmpp.Communication
{
    public class XmppChannelFactory : IChannelFactory
    {
        private StreamChannelFactory streamChannelFactory;

        public XmppChannelFactory(StreamChannelFactory streamChannelFactory)
        {
            this.streamChannelFactory = streamChannelFactory;
        }

        public IChannel CreateChannel(ConnectionSettings settings)
        {
            StreamInitializer initializer = new StreamInitializer(settings);
            initializer.Negotiators.Add(CreateSaslNegotiator(settings));
            XmppChannel channel = new XmppChannel(initializer, streamChannelFactory.CreateChannel(settings), settings);
            initializer.Negotiators.Add(new TlsNegotiator(channel.SetTlsContext));
            initializer.Negotiators.Add(new BindNegotiator(settings));
            
            return channel;
        }

        private SaslNegotiator CreateSaslNegotiator(ConnectionSettings settings)
        {
            SaslNegotiator sasl = new SaslNegotiator(settings);
            sasl.AddAuthenticator("PLAIN", Authenticators.Plain);
            return sasl;
        }
    }    
}
