using Redcat.Core;
using Redcat.Core.Communication;
using Redcat.Xmpp.Negotiators;
using StreamChannelFactory = Redcat.Core.Communication.IChannelFactory<Redcat.Core.Communication.IStreamChannel>;

namespace Redcat.Xmpp.Services
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
            initializer.Negotiators.Add(CreateSaslNegotiator());
            XmppChannel channel = new XmppChannel(initializer, streamChannelFactory.CreateChannel(settings), settings);
            initializer.Negotiators.Add(new TlsNegotiator(channel.SetTlsContext));
            
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
