using Redcat.Core;
using Redcat.Core.Channels;
using Redcat.Xmpp.Negotiators;
using System;
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
            IStreamChannel streamChannel = streamChannelFactory.CreateChannel(settings);
            XmppChannel channel = new XmppChannel(streamChannel, settings);
            channel.StreamInitializer = CreateInitializer(channel.SetTlsContext, settings);
            
            return channel;
        }

        private Action<IXmppStream> CreateInitializer(Action setTlsContext, ConnectionSettings settings)
        {
            StreamInitializer initializer = new StreamInitializer(settings);
            //initializer.Negotiators.Add(CreateSaslNegotiator(settings));
            initializer.Negotiators.Add(new TlsNegotiator(setTlsContext));
            initializer.Negotiators.Add(new BindNegotiator(settings));
            initializer.Negotiators.Add(new RegistrationNegotiator());
            return initializer.Init;
        }

        private SaslNegotiator CreateSaslNegotiator(ConnectionSettings settings)
        {
            SaslNegotiator sasl = new SaslNegotiator(settings);
            sasl.AddAuthenticator("PLAIN", Authenticators.Plain);
            return sasl;
        }
    }    
}
