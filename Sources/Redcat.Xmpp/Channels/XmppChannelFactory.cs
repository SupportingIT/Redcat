using Redcat.Core;
using Redcat.Core.Channels;
using Redcat.Xmpp.Negotiators;
using System;

namespace Redcat.Xmpp.Channels
{
    public class XmppChannelFactory : IXmppChannelFactory
    {
        private IStreamChannelFactory streamChannelFactory;
        private Func<ISaslCredentials> credentialsProvider;

        public XmppChannelFactory(IStreamChannelFactory streamChannelFactory, Func<ISaslCredentials> credentialsProvider)
        {
            this.streamChannelFactory = streamChannelFactory;
            this.credentialsProvider = credentialsProvider;
        }

        public IXmppChannel CreateChannel(ConnectionSettings settings)
        {            
            //IStreamChannel streamChannel = streamChannelFactory.CreateChannel(settings);
            XmppChannel channel = new XmppChannel(null, settings);
            //channel.Initializer = CreateInitializer(channel.SetTlsContext, settings, credentialsProvider);
            return channel;
        }

        private Func<IXmppStream, NegotiationContext> CreateInitializer(Action setTlsContext, ConnectionSettings settings, Func<ISaslCredentials> credentialsProvider)
        {
            StreamInitializer initializer = new StreamInitializer(settings);
            initializer.Negotiators.Add(CreateSaslNegotiator(credentialsProvider));
            initializer.Negotiators.Add(new TlsNegotiator(setTlsContext));
            initializer.Negotiators.Add(new BindNegotiator());
            initializer.Negotiators.Add(new SessionNegotiator());
            //initializer.Negotiators.Add(new RegistrationNegotiator());
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
