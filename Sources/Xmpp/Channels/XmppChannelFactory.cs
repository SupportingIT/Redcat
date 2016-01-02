using Redcat.Core;
using Redcat.Core.Channels;
using Redcat.Xmpp.Negotiators;
using StreamChannelFactory = Redcat.Core.Channels.IChannelFactory<Redcat.Core.Channels.IStreamChannel>;

namespace Redcat.Xmpp.Channels
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
            //initializer.Negotiators.Add(CreateSaslNegotiator(settings));
            XmppChannel channel = new XmppChannel(initializer, streamChannelFactory.CreateChannel(settings), settings);
            initializer.Negotiators.Add(new TlsNegotiator(channel.SetTlsContext));
            initializer.Negotiators.Add(new BindNegotiator(settings));
            initializer.Negotiators.Add(new RegistrationNegotiator());
            
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
