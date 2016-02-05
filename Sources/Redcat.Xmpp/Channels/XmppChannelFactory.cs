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
        private Func<ISaslCredentials> credentialsProvider;

        public XmppChannelFactory(StreamChannelFactory streamChannelFactory, Func<ISaslCredentials> credentialsProvider)
        {
            this.streamChannelFactory = streamChannelFactory;
            this.credentialsProvider = credentialsProvider;
        }

        public IChannel CreateChannel(ConnectionSettings settings)
        {            
            IStreamChannel streamChannel = streamChannelFactory.CreateChannel(settings);
            XmppChannel channel = new XmppChannel(streamChannel, settings);
            channel.StreamInitializer = CreateInitializer(channel.SetTlsContext, settings, credentialsProvider);
            
            return channel;
        }

        private Action<IXmppStream> CreateInitializer(Action setTlsContext, ConnectionSettings settings, Func<ISaslCredentials> credentialsProvider)
        {
            StreamInitializer initializer = new StreamInitializer(settings);
            initializer.Negotiators.Add(CreateSaslNegotiator(credentialsProvider));
            initializer.Negotiators.Add(new TlsNegotiator(setTlsContext));
            initializer.Negotiators.Add(new BindNegotiator());
            initializer.Negotiators.Add(new RegistrationNegotiator());
            return initializer.Init;
        }

        private SaslNegotiator CreateSaslNegotiator(Func<ISaslCredentials> credentialsProvider)
        {
            SaslNegotiator sasl = new SaslNegotiator(credentialsProvider);
            sasl.AddAuthenticator("PLAIN", Authenticators.Plain);
            return sasl;
        }
    }    
}
